using System;
using System.Collections.Concurrent;
using System.Collections.ObjectModel;
using System.Threading;
using System.Threading.Tasks;
using Avalonia.Threading;
using ReactiveUI;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Avalonia.Controls;

namespace Homework4.ViewModels
{
    //main window: manages recipes, simulation commands, and speed
    public partial class MainWindowViewModel : HybridViewModel
    {

        /*
        public ObservableCollection<ActiveRecipe> _activeRecipes = new();
        */
        //recipes currently being simulated
        // [ObservableProperty] private ObservableCollection<ActiveRecipe> _activeRecipes = new();
        [ObservableProperty] private ObservableCollection<ActiveRecipe> _activeRecipes = [];
        [ObservableProperty] private ObservableCollection<Recipe> _loadedRecipes = [];
        [ObservableProperty] private ObservableCollection<Ingredient> _ingredients = [];
        [ObservableProperty] private ObservableCollection<ActiveRecipe> _completedRecipes = [];
        /*public ObservableCollection<ActiveRecipe> CompletedRecipes 
        { 
            get => _completedRecipes;
            set=> this.RaiseAndSetIfChanged(ref _completedRecipes, value); 
        }*/

        [ObservableProperty] private ConcurrentQueue<ActiveRecipe> _queue = new();

        private double _speed = 1.0;
        // Speed multiplier for the simulation (1.0 = real time)
        public double Speed
        {
            get => _speed;
            set => this.RaiseAndSetIfChanged(ref _speed, value);
        }
        // Cancellation token source to stop or restart the simulation tasks


        [RelayCommand]
        public async Task StartSimulationCommand() => await Task.Run(StartSimulation);
        /*
        [RelayCommand]
        public async Task StopSimulationCommand() => await Task.Run(StopSimulation);*/
        public MainWindowViewModel()
        {
            ActiveRecipe._viewModel = this;
        }

        // Adds a new recipe to the simulation list


        // Starts or restarts the simulation by cancelling old tasks and launching new ones
        public async void StartSimulation()
        {
            foreach (var recipe in ActiveRecipes)
                _ = SimulateRecipeAsync(recipe);
        }

        // Cancels all running simulation tasks
        public async void HandleNewRecipeCommand(object? parameter, SelectionChangedEventArgs e)
        {
            // if (e.AddedItems[0] ! is Recipe || e.AddedItems[0] is null) return;
            await Task.Run(() =>
            {
                Queue.Enqueue(new ActiveRecipe((Recipe)e.AddedItems[0]));
                
            }).ContinueWith(t=>NotifyQueueChanged());
        }

        private async void NotifyQueueChanged()
        {
            if (ActiveRecipes.Count < 3)
            {
                await NotifyFreeSpot();
            }
            else return;
        }
        private async Task NotifyFreeSpot()
        {
            if (Queue.IsEmpty) return;
            await Task.Run(() =>
            {
                Queue.TryDequeue(out var recipe);
                ActiveRecipes.Add(recipe ?? throw new InvalidOperationException("Queue is empty"));
                SimulateRecipeAsync(recipe);
            });
        }

        public async void CancelRecipeCommand(ActiveRecipe recipe)
        {
            await recipe.CancellationToken.CancelAsync();
            Dispatcher.UIThread.Post(() => ActiveRecipes.Remove(recipe));
            await NotifyFreeSpot();
        }

        // Runs the step-by-step simulation for a single recipe
        private async Task SimulateRecipeAsync(ActiveRecipe recipe)
        {
            var token = recipe.CancellationToken.Token;
            await Task.Run(async() =>
            {
                try
                {
                    foreach (var step in recipe.Steps)
                    {
                        token.ThrowIfCancellationRequested(); // Check cancellation before each step
            
                        Dispatcher.UIThread.Post(() => recipe.CurrentStep = step.Step);
                        var stepMs = (int)(step.Duration * 500 / Speed);
            
                        await Task.Delay(stepMs, token); // Respect cancellation
                        Dispatcher.UIThread.Post(() => recipe.Progress = 100); // Example progress update
                    }
                }
                catch (OperationCanceledException)
                {
                    // Handle cancellation gracefully
                    Dispatcher.UIThread.Post(() => recipe.CurrentStep = "Cancelled");
                    Dispatcher.UIThread.Post(() => CompletedRecipes.Add(recipe));
                    
                }
                finally
                {
                    Dispatcher.UIThread.Post(() => ActiveRecipes.Remove(recipe));
                    await NotifyFreeSpot();
                }
                
            }, token).ContinueWith(async t => {                     // Mark recipe as done, then remove it
                Dispatcher.UIThread.Post(() => recipe.CurrentStep = "Done");
                Dispatcher.UIThread.Post(() => ActiveRecipes.Remove(recipe));
                await NotifyFreeSpot();
                Dispatcher.UIThread.Post(() => CompletedRecipes.Add(recipe));
            }, token);
        }
        
        
    }
}