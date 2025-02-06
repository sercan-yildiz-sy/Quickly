using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using Quicky.Models;
using SQLite;

namespace Quicky.Data
{
    public static class QuickyService
    {
        static SQLiteAsyncConnection db;
        static async Task Init() {
            if (db != null) 
                return;
            
            var databasePath = Path.Combine(FileSystem.AppDataDirectory, "MyData.db");

            db = new SQLiteAsyncConnection(databasePath);

            await db.CreateTableAsync<Item>();
        }

        public static async Task AddItem(string Name, float Quantity,  string Quantity_Type,  string Image, string Location)
        {
            await Init();
            var item = new Item
            {
                Name = Name,
                Quantity = Quantity,
                Quantity_Type = Quantity_Type,
                Image = Image,
                Location = Location
            };
            await db.InsertAsync(item);
        }
        public static async Task DeleteItem(int Id)
        {
            await Init();

            await db.DeleteAsync<Item>(Id);
        }
        public static async Task<IEnumerable<Item>> GetItem()
        {
            await Init();

            var item = await db.Table<Item>().ToListAsync();
            return item;
        }
    }
}
