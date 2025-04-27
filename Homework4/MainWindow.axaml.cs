using Avalonia.Controls;
using Avalonia.Interactivity;
using Homework4.ViewModels;
using System;

namespace Homework4
{
    public partial class MainWindow : Window
    {
        private readonly MainWindowViewModel _vm;

        public MainWindow()
        {
            InitializeComponent();             // Load XAML contents
            _vm = new MainWindowViewModel();   // Create ViewModel instance
            DataContext = _vm;                 // Bind UI to ViewModel
        }

        // Handles the Speed button click: opens dialog, retrieves speed multiplier
        private async void OnSpeedButtonClick(object sender, RoutedEventArgs e)
        {
            var dialog = new SpeedDialog();    // Create speed input dialog
            await dialog.ShowDialog(this);     // Show as modal window

            if (dialog.Speed > 0)
            {
                _vm.Speed = dialog.Speed;      // Update ViewModel speed
                ((Button)sender).Content =      // Update button text to reflect new speed
                    $"Speed ({dialog.Speed}x)";
            }
        }

        // Handles New Order button: opens file-picker, then adds selected recipes
        private async void OnNewOrderClick(object sender, RoutedEventArgs e)
        {
            try
            {
                // Center the new order window over the main window
                var newOrderWindow = new NewOrderWindow
                {
                    WindowStartupLocation = WindowStartupLocation.CenterOwner
                };

                await newOrderWindow.ShowDialog(this);  // Show as modal
                // Add each selected recipe to the simulation
                foreach (var recipe in newOrderWindow.SelectedRecipes)
                    _vm.AddRecipe(recipe);
            }
            catch (Exception ex)
            {
                // Log any errors (could improve by showing a UI message)
                Console.WriteLine($"ERROR: {ex}");
            }
        }
    }
}

// This file contains the event handlers for MainWindow (Speed and New Order interactions)
// and wires the window to its ViewModel for data binding and command execution.
