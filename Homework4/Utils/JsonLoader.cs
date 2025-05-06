using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using System.Text.Json;

namespace Homework4
{
    public static class JsonLoader
    {
        /// <summary>
        /// Loads all data from a JSON file asynchronously into a RootObject
        /// </summary>
        /// <param name="filePath">Path to the JSON file</param>
        /// <returns>A RootObject containing ingredients and recipes</returns>
        public static async Task<RootObject> LoadDataAsync(Uri filePath)
        {
            var stringPath = filePath.LocalPath.Replace("%20", " ");
            if (!File.Exists(stringPath))
            {
                Console.WriteLine($"File not found: {filePath}");
                return new RootObject();
            }

            Console.WriteLine($"Loading data from: {filePath}");

            try
            {
                // Open file stream
                using FileStream fileStream = new(stringPath, FileMode.Open, FileAccess.Read, FileShare.Read);

                // Deserialize the entire JSON into a RootObject
                var options = new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true,
                    AllowTrailingCommas = true,
                    ReadCommentHandling = JsonCommentHandling.Skip
                };

                var rootObject = await JsonSerializer.DeserializeAsync<RootObject>(fileStream, options);
                return rootObject ?? new RootObject();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error loading data: {ex.Message}");
                return new RootObject();
            }
        }

        /// <summary>
        /// Loads only the ingredients from a JSON file asynchronously
        /// </summary>
        /// <param name="filePath">Path to the JSON file</param>
        /// <returns>A list of ingredients</returns>
        public static async Task<List<Ingredient>> LoadIngredientsAsync(string filePath)
        {
            var rootObject = await LoadDataAsync(new(filePath));
            return rootObject.Ingredients;
        }

        /// <summary>
        /// Loads only the recipes from a JSON file asynchronously
        /// </summary>
        /// <param name="filePath">Path to the JSON file</param>
        /// <returns>A list of recipes</returns>
        public static async Task<List<Recipe>> LoadRecipesAsync(string filePath)
        {
            var rootObject = await LoadDataAsync(new(filePath));
            return rootObject.Recipes;
        }


    }
}