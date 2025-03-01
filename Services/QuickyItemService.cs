using System.Text.Json;
using Quicky.Models;

namespace Quicky.Services
{
    public static class QuickyItemService
    {
        public static async Task<List<Item>> GetItems()
        {

            try
            {
                await using var stream = await FileSystem.OpenAppPackageFileAsync("items.json");
                var itemJson = await JsonSerializer.DeserializeAsync<ItemJson>(stream);
                return itemJson?.Items ?? new List<Item>();
            }

            catch (Exception ex)
            {
                Console.WriteLine($"Error loading items: {ex.Message}");
                return new List<Item>();
            }
        }
    }
}
