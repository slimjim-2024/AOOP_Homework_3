using System.Collections.ObjectModel;

namespace Homework4
{
    // Holds recipes loaded from a JSON file
    public static class DataService
    {
        // Shared list of recipes; UI binds here to display available recipes
        public static ObservableCollection<Recipe> LoadedRecipes { get; } = new();
        public static ObservableCollection<Ingredient> Ingredients { get; } = new();
    }
}