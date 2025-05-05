using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Avalonia.Threading;
using ReactiveUI;
using System.Reactive;

namespace Homework4.ViewModels
{
    //main window: manages recipes, simulation commands, and speed
    public class MainWindowViewModel : ReactiveObject
    {
        //recipes currently being simulated
        public ObservableCollection<ActiveRecipe> ActiveRecipes { get; } = new();

        private int _activeRecipesCount;
        // Exposes the count of active recipes for display in the header
        public int ActiveRecipesCount
        {
            get => _activeRecipesCount;
            private set => this.RaiseAndSetIfChanged(ref _activeRecipesCount, value);
        }

        private double _speed = 1.0;
        // Speed multiplier for the simulation (1.0 = real time)
        public double Speed
        {
            get => _speed;
            set => this.RaiseAndSetIfChanged(ref _speed, value);
        }
        // Cancellation token source to stop or restart the simulation tasks

        private CancellationTokenSource _simulationCts = new();
        // Command to start the simulation; bound to the Start button
        public ReactiveCommand<Unit, Unit> StartSimulationCommand { get; }
        // Command to stop the simulation; bound to the Stop button
        public ReactiveCommand<Unit, Unit> StopSimulationCommand  { get; }

        public MainWindowViewModel()
        {
            ActiveRecipes.CollectionChanged += (s, e) =>
                ActiveRecipesCount = ActiveRecipes.Count;

            // Create Start and Stop commands, ensuring CanExecuteChanged fires on UI thread
            StartSimulationCommand = ReactiveCommand.Create(
                execute: StartSimulation,
                canExecute: null,
                outputScheduler: RxApp.MainThreadScheduler
            );
            StopSimulationCommand = ReactiveCommand.Create(
                execute: StopSimulation,
                canExecute: null,
                outputScheduler: RxApp.MainThreadScheduler
            );
        }

        // Adds a new recipe to the simulation list
        public void AddRecipe(Recipe recipe)
        {
            var active = new ActiveRecipe(recipe, r =>
            {
                // Ensure removal happens on UI thread
                Dispatcher.UIThread.Post(() => ActiveRecipes.Remove(r));
            });
            ActiveRecipes.Add(active);
        }

        // Starts or restarts the simulation by cancelling old tasks and launching new ones
        public void StartSimulation()
        {
            _simulationCts?.Cancel();
            _simulationCts = new CancellationTokenSource();

            foreach (var recipe in ActiveRecipes.ToList())
                _ = SimulateRecipeAsync(recipe, _simulationCts.Token);
        }

        // Cancels all running simulation tasks
        public void StopSimulation() => _simulationCts.Cancel();

        // Runs the step-by-step simulation for a single recipe
        private async Task SimulateRecipeAsync(ActiveRecipe recipe, CancellationToken token)
        {
            try
            {
                foreach (var step in recipe.Steps)
                {
                    // Check for cancellation before starting a new step
                    Dispatcher.UIThread.Post(() => recipe.CurrentStep = step.Step);

                    // Calculate the target elapsed time based on step duration and speed
                    var stepMs = (int)(step.Duration * 1000 / Speed);
                    var target = recipe.ElapsedTime + stepMs;
                    const int interval = 100;

                    // Simulate the step by incrementing elapsed time in intervals
                    while (recipe.ElapsedTime < target)
                    {
                        token.ThrowIfCancellationRequested();
                        await Task.Delay(interval, token);
                        Dispatcher.UIThread.Post(() =>
                            recipe.ElapsedTime = Math.Min(target, recipe.ElapsedTime + interval)
                        );
                    }
                }

                // Mark recipe as done, then remove it
                Dispatcher.UIThread.Post(() => recipe.CurrentStep = "Done");
                await Task.Delay(500, token);
                Dispatcher.UIThread.Post(() => ActiveRecipes.Remove(recipe));
            }
            catch (OperationCanceledException)
            {
                // Expected when StopSimulation is called
            }
            catch
            {
                // On error, ensure cleanup of this recipe
                Dispatcher.UIThread.Post(() => ActiveRecipes.Remove(recipe));
            }
        }
        
    }
}
