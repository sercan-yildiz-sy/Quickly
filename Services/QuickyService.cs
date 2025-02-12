using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using Quicky.Models;
using SQLite;

namespace Quicky.Services
{
    public static class QuickyService
    {
        static SQLiteAsyncConnection db;
        static async Task Init() {
            if (db != null) 
                return;
            
            var databasePath = Path.Combine(FileSystem.AppDataDirectory, "MyData.db");

            db = new SQLiteAsyncConnection(databasePath);

            await db.CreateTableAsync<Inventory>();
        }

        public static async Task AddInventory(int Id, string Name, float Quantity, string Quantity_Type, string Image, string Location, string Category)
        {
            await Init();
            var item = new Inventory
            {
                Id = Id,
                Name = Name,
                Quantity = Quantity,
                Quantity_Type = Quantity_Type,
                Image = Image,
                Location = Location,
                Category = Category
            };
            await db.InsertAsync(item);
        }
        public static async Task DeleteInventory(int Id)
        {
            await Init();

            await db.DeleteAsync<Inventory>(Id);
        }
        public static async Task<IEnumerable<Inventory>> GetInventory()
        {
            await Init();

            var item = await db.Table<Inventory>().ToListAsync();
            return item;
        }
    }
}
