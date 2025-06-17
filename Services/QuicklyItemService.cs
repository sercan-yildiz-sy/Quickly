using System.Text.Json;
using Quickly.Models;

namespace Quickly.Services
{

    /// <summary>
    /// Provides methods to retrieve item data from a local JSON file.
    /// </summary>
    public static class QuicklyItemService
    {
        /// <summary>
        /// Asynchronously loads and returns a list of items from the embedded items.json file.
        /// </summary>
        /// <returns>
        /// A list of <see cref="Item"/> objects. Returns an empty list if loading or deserialization fails.
        /// </returns>
        public static async Task<List<Item>> GetItems()
        {

            try
            {
                // Open the embedded items.json file as a stream
                await using var stream = await FileSystem.OpenAppPackageFileAsync("items.json");
                // Deserialize the JSON content into an ItemJson object
                var itemJson = await JsonSerializer.DeserializeAsync<ItemJson>(stream);
                // Return the list of items, or an empty list if null
                return itemJson?.Items ?? new List<Item>();
            }

            catch (Exception ex)
            {
                // Log the error and return an empty list if any exception occurs
                Console.WriteLine($"Error loading items: {ex.Message}");
                return new List<Item>();
            }
        }
    }
}
