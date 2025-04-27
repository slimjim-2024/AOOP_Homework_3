using System;
using System.Collections.Generic;
using System.Linq;
using ReactiveUI;
using System.Reactive;

namespace Homework4
{
    // Represents a recipe in progress, with timing and removal support
    public class ActiveRecipe : ReactiveObject
    {
        // Recipe name
        public string Name { get; }

        // Steps to complete, each with a duration
        public List<RecipeStep> Steps { get; }

        // Total duration (sum of all steps) in milliseconds
        public int TotalDuration { get; }

        private int _elapsedTime;
        // Time elapsed so far in milliseconds
        public int ElapsedTime
        {
            get => _elapsedTime;
            set => this.RaiseAndSetIfChanged(ref _elapsedTime, value);
        }

        // Progress percentage (0-100)
        public double Progress => TotalDuration > 0
            ? (double)ElapsedTime / TotalDuration * 100
            : 0;

        private string _currentStep = string.Empty;
        // Description of the current step
        public string CurrentStep
        {
            get => _currentStep;
            set => this.RaiseAndSetIfChanged(ref _currentStep, value);
        }

        // Command to remove this recipe from the active list
        public ReactiveCommand<Unit, Unit> RemoveCommand { get; }

        // Construct with a model and a removal callback
        public ActiveRecipe(Recipe recipe, Action<ActiveRecipe> removeAction)
        {
            Name = recipe.Name ?? throw new ArgumentNullException(nameof(recipe));
            Steps = recipe.Steps ?? throw new ArgumentNullException(nameof(recipe.Steps));
            TotalDuration = Steps.Sum(s => s.Duration * 1000);

            // Set up removal command on the UI thread
            RemoveCommand = ReactiveCommand.Create(
                () => removeAction(this),
                canExecute: null,
                outputScheduler: RxApp.MainThreadScheduler
            );
        }
    }
}

// This file defines ActiveRecipe, handling runtime state (ElapsedTime, CurrentStep, Progress)
// and providing a RemoveCommand for UI interaction.
