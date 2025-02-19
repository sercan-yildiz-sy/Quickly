using System.Text.Json;
using Quicky.Models;

namespace Quicky.Services
{
    public class QuickyItemService
    {
        public async Task<List<Item>> GetItems()
        {

            try
            {
                await using var stream = await FileSystem.OpenAppPackageFileAsync("items.json");
                return await JsonSerializer.DeserializeAsync<List<Item>>(stream) ?? new List<Item>();
            }

            catch (Exception ex)
            {
                Console.WriteLine($"Error loading items: {ex.Message}");
                return new List<Item>();
            }
        }
    }
}
