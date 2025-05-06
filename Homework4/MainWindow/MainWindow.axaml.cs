using Avalonia.Controls;
using Avalonia.Interactivity;
using Homework4.ViewModels;
using System;
using System.Linq;
using System.Threading.Tasks;
using Avalonia.Platform.Storage;

namespace Homework4
{
    public partial class MainWindow : Window
    {
        private readonly MainWindowViewModel _vm = new MainWindowViewModel();

        public MainWindow()
        {
            InitializeComponent();             // Load XAML contents
            DataContext = _vm;                 // Bind UI to ViewModel
            AvailableRecipes.SelectionChanged += _vm.HandleNewRecipeCommand;
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
                //var newOrderWindow = new NewOrderWindow
                //{
                //    WindowStartupLocation = WindowStartupLocation.CenterOwner
                //};

                //await newOrderWindow.ShowDialog(this);  // Show as modal
                //// Add each selected recipe to the simulation
                //foreach (var recipe in newOrderWindow.SelectedRecipes)
                //    _vm.AddRecipe(recipe);
            }
            catch (Exception ex)
            {
                // Log any errors (could improve by showing a UI message)
                Console.WriteLine($"ERROR: {ex}");
            }
        }

        private async void OnLoadJsonClick(object sender, RoutedEventArgs e)
        {
            try
            {
                var storageProvider = StorageProvider;
                var result = await storageProvider.OpenFilePickerAsync(new FilePickerOpenOptions
                {
                    Title = "Select Recipe File",
                    AllowMultiple = false,
                    FileTypeFilter = new[]
                    {
                        new FilePickerFileType("JSON Files")
                        {
                            Patterns = new[] { "*.json" }
                        }
                    }
                });

                if (result.Count == 1)
                {
                    await Task.Run(async() =>
                    {
                        var data = await JsonLoader.LoadDataAsync(result.First().Path);
                        foreach (var item in data.Recipes) 
                            {
                                _vm.LoadedRecipes.Add(item);
                        
                            }
                        foreach (var item in data.Ingredients)
                        {
                            DataService.Ingredients.Add(item);
                        }

                    });
                   
                }
            }
            catch (Exception ex)
            {
                // Show error dialog if JSON loading fails
                var dialog = new MessageBox("Error", $"Failed to load JSON: {ex.Message}");
                await dialog.ShowDialog(this);
            }
        }
    }
}

// This file contains the event handlers for MainWindow (Speed and New Order interactions)
// and wires the window to its ViewModel for data binding and command execution.
