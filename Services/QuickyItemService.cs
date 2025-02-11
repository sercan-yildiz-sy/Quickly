using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Quicky.Models;

namespace Quicky.Services
{
    public class QuickyItemService
    {
        List<Item> ItemList;

        public async Task<List<Item>> GetItems()
        {
            using var stream = await FileSystem.OpenAppPackageFileAsync("items.json");
            var items = await JsonSerializer.DeserializeAsync<List<Item>>(stream);
            return items ?? new List<Item>();
        }
    }
}
