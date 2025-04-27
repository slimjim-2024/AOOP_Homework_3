using System.Collections.Generic;

namespace Homework4
{
    public class RootObject
    {
        // List of available ingredients from the JSON
        public List<Ingredient> Ingredients { get; set; } = new();
        // List of recipes, each containing steps and equipment
        public List<Recipe> Recipes { get; set; } = new();
    }

    // Represents a ingredient in the JSON
    public class Ingredient
    {
        public string Name { get; set; } = string.Empty;    // Ingredient name, "Tomato"
        public string Quantity { get; set; } = string.Empty;// Amount of ingredient, "10"
        public string Unit { get; set; } = string.Empty;    // Unit type, "pcs" or "grams"
    }

    public class Recipe
    {
        public string Name { get; set; } = string.Empty;       // Recipe name, "Pasta Pomodoro"
        public string Difficulty { get; set; } = string.Empty; // Difficulty level, "Easy"
        public List<string> Equipment { get; set; } = new();   // Required equipment names
        public List<RecipeStep> Steps { get; set; } = new();   // Ordered list of steps with durations
    }

    public class RecipeStep
    {
        public string Step { get; set; } = string.Empty;    // Description of the step, "Boil water"
        public int Duration { get; set; }                  // Duration in seconds
    }
}

//file responsible for loading and parsing JSON data into C# objects. 
// Ingredients and Recipes arrays, and Ingredient/Recipe/RecipeStep from the JSON file