using System.Diagnostics;
using Quickly.Models;
using SQLite;

namespace Quickly.Services
{
    public static class QuicklyService
    {
        static SQLiteAsyncConnection db;
        static async Task Init() {
            if (db != null) 
                return;
            
            var databasePath = Path.Combine(FileSystem.AppDataDirectory, "MyData.db");

            db = new SQLiteAsyncConnection(databasePath);
            await db.CreateTableAsync<Inventory>();
        }


        public static async Task<Inventory> AddInventory(int ItemId, string Name, string Image, float Quantity, string Quantity_Type, string Category, string Location)
        {
            await Init();
            
            var item = new Inventory
            {
                ItemId = ItemId,
                Name = Name,
                Image = Image,
                Quantity = Quantity,
                Quantity_Type = Quantity_Type,
                Category = Category,
                Location = Location
                };
            await db.InsertAsync(item);

            return item;
        }

        public static async Task DeleteInventory(int Id)
        {
            
            await Init();
            await db.DeleteAsync<Inventory>(Id);
        }

        public static async Task<IEnumerable<Inventory>> GetInventory(string Location = "All")
        {

            await Init();
            if (Location == "All")
            {
                return await db.Table<Inventory>().ToListAsync();
            }
            else
            {
                return await db.Table<Inventory>().Where(i  => i.Location == Location).ToListAsync();
            }

        }

        public static async Task UpdateInventory(int Id, float Quantity, string Quantity_Type, string Location)
        {
            await Init();
            var item = await db.FindAsync<Inventory>(Id);
            if (item != null)
            {
                item.Quantity = Quantity;
                item.Quantity_Type = Quantity_Type;
                item.Location = Location;
                Debug.WriteLine($"Updating Inventory: Id={Id}, Quantity={Quantity}, Quantity_Type={Quantity_Type}, Location={Location}");
                try
                {
                    await db.UpdateAsync(item);
                    Debug.WriteLine("Inventory updated successfully.");
                }
                catch (Exception ex)
                {
                    Debug.WriteLine($"Error updating inventory: {ex.Message}");
                }
            }
            else
            {
                Debug.WriteLine($"Inventory item with Id={Id} not found.");
            }
        }


        public static async Task<Inventory> GetInventoryItem(int Id)
        {
            await Init();
            return await db.FindAsync<Inventory>(Id);
        }
            
    } 
}



