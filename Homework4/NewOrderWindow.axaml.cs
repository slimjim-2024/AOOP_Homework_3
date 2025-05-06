
//using Avalonia.Controls;
//using Avalonia.Interactivity;
//using Avalonia.Platform.Storage;
//using System;
//using System.IO;
//using System.Linq;
//using System.Text.Json;
//using System.Collections.Generic;
//using System.Threading.Tasks;

//namespace Homework4
//{
//    // Modal window for loading recipes from JSON and selecting orders
//    public partial class NewOrderWindow : Window
//    {
//        // List of recipes chosen by the user to start simulation
//        public List<Recipe> SelectedRecipes { get; private set; } = new();

//        public NewOrderWindow()
//        {
//            InitializeComponent();
//            RecipesList.ItemsSource = DataService.LoadedRecipes;
//        }

//        // Triggered when the user clicks "Load JSON"; opens file picker and recipes
//        private async void OnLoadJsonClick(object sender, RoutedEventArgs e)
//        {
//            try
//            {
//                var storageProvider = StorageProvider;
//                var result = await storageProvider.OpenFilePickerAsync(new FilePickerOpenOptions
//                {
//                    Title = "Select Recipe File",
//                    AllowMultiple = false,
//                    FileTypeFilter = new[]
//                    {
//                        new FilePickerFileType("JSON Files")
//                        {
//                            Patterns = new[] { "*.json" }
//                        }
//                    }
//                });

//                if (result.Count == 1)
//                {
//                    await Task.Run(async() =>
//                    {
//                        var data = await JsonLoader.LoadDataAsync(result.First().Path);
//                        foreach (var item in data.Recipes) 
//                            {
//                                DataService.LoadedRecipes.Add(item);
                        
//                            }
//                        foreach (var item in data.Ingredients)
//                        {
//                            DataService.Ingredients.Add(item);
//                        }

//                    });



//                    /*await using var stream = await result[0].OpenReadAsync();
//                    using var reader = new StreamReader(stream);
//                    var json = await reader.ReadToEndAsync();

//                    var options = new JsonSerializerOptions
//                    {
//                        PropertyNameCaseInsensitive = true,
//                        AllowTrailingCommas = true
//                    };

//                    var root = JsonSerializer.Deserialize<RootObject>(json, options);

//                    // Update the shared recipe list for other windows to use
//                    DataService.LoadedRecipes.Clear();
//                    foreach (var recipe in root?.Recipes ?? new List<Recipe>())
//                    {
//                        DataService.LoadedRecipes.Add(recipe);
//                    }*/
//                }
//            }
//            catch (Exception ex)
//            {
//                // Show error dialog if JSON loading fails
//                var dialog = new MessageBox("Error", $"Failed to load JSON: {ex.Message}");
//                await dialog.ShowDialog(this);
//            }
//        }

//        // Called when user confirms their selection; captures selected recipes
//        private void OnAddOrderClick(object sender, RoutedEventArgs e)
//        {
//            SelectedRecipes = RecipesList.SelectedItems
//                ?.Cast<Recipe>()
//                .ToList() ?? new List<Recipe>();

//            Close();
//        }

//        // Closes the window without selecting any recipes
//        private void OnCancelClick(object sender, RoutedEventArgs e) => Close();
//    }
//}

//// This file implements NewOrderWindow, which lets the user load recipes from a JSON file
//// into DataService and then pick which recipes to add as new orders for simulation.