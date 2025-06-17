using System.Diagnostics;
using Quickly.Models;
using SQLite;

namespace Quickly.Services
{

    /// <summary>
    /// This class provides asynchronous CRUD operations for Inventory items using SQLite
    /// </summary>
    public static class QuicklyService
    {

        // SQLite connection instance
        static SQLiteAsyncConnection db;

        /// <summary>
        /// Initializes the SQLite database connection and creates the Inventory table if it doesn't exist
        /// </summary>
        static async Task Init() {
            if (db != null) 
                return;
            
            var databasePath = Path.Combine(FileSystem.AppDataDirectory, "MyData.db");

            db = new SQLiteAsyncConnection(databasePath);
            await db.CreateTableAsync<Inventory>();
        }

        /// <summary>
        /// Adds a new inventory item to the database  
        /// </summary>
        /// <param name="ItemId">The item ID</param>
        /// <param name="Name">The name of the item</param>
        /// <param name="Image">The image path</param>
        /// <param name="Quantity">The quantity of the item</param>
        /// <param name="Quantity_Type">The type of quantity (e.g., kg, unit)</param>
        /// <param name="Category">The category of the item</param>
        /// <param name="Location">The storage location</param>
        /// <returns>The newly added <see cref="Inventory"/> item</returns>
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

        /// <summary>
        /// Deletes an item from the inventory based on its Id.
        /// </summary>
        /// <param name="Id"></param>
        public static async Task DeleteInventory(int Id)
        {
            
            await Init();
            await db.DeleteAsync<Inventory>(Id);
        }

        /// <summary>
        /// Retrieves all inventory items or items from a specific location.
        /// </summary>
        ///  <param name="Location"> The location to filter the inventory items by. If "All", retrieves all items.</param>
        ///  <returns>A collection of <see cref="Inventory"/> items.</returns>
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

        /// <summary>
        /// The method <c>UpdateInventory</c> updates the quantity, quantity type, and location of an existing inventory item based on its Id.
        /// </summary> 
        /// <param name="Id">The ID of the inventory item to update.</param>
        /// <param name="Quantity">The new quantity.</param>
        /// <param name="Quantity_Type">The new quantity type.</param>
        /// <param name="Location">The new location.</param>
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

        /// <summary>
        /// The method <c>GetInventoryItem</c> retrieves a specific inventory item by its Id.
        /// </summary>
        /// <param name="Id">The ID of the inventory item.</param>
        /// <returns>The <see cref="Inventory"/> item or null if not found</returns>
        public static async Task<Inventory> GetInventoryItem(int Id)
        {
            await Init();
            return await db.FindAsync<Inventory>(Id);
        }
            
    } 
}



