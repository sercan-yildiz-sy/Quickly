using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Quickly.Models;
using Quickly.Services;
using System.Diagnostics;
using System.Threading.Tasks;

namespace Quickly.PageModels
{
    /// <summary>
    /// ViewModel for managing item details in the inventory.
    /// Handles loading, updating, and deleting inventory items.
    /// </summary>
    [QueryProperty(nameof(InventoryId), "id")]
    public partial class ItemDetailsPageModel : ObservableObject, IBaseClass
    {
        /// Indicates whether the view model is performing a busy operation (e.g., loading or updating items).
        [ObservableProperty]
        public bool _isBusy;

        /// The inventory item being displayed or edited.
        [ObservableProperty]
        public Inventory _inventory = new Inventory();

        // Backing field for InventoryId property
        private int _inventoryId;

        /// The ID of the inventory item to load. Setting this triggers loading the item.
        public int InventoryId
        {
            get => _inventoryId;
            set
            {
                _inventoryId = value;
                _ = LoadInventory(value);
            }
        }

        /// List of available quantity types for inventory items.
        [ObservableProperty]
        private List<string> _quantityTypes = new List<string>
            {
                "Unit", "kg", "g", "lb", "lt", "ml", "oz", "pack", "can", "bottle", "bag", "box"
            };

        /// List of possible storage locations for inventory items.
        [ObservableProperty]
        private List<string> _locations = new List<string>
            {
                "Pantry", "Fridge", "Freezer"
            };

        /// List of inventory categories.
        [ObservableProperty]
        private List<string> _categories = new List<string>
            {
                "All Items", "Produce", "Meat", "Dairy", "Canned Products", "Dry Products", "Frozen Products",
                "Seafood", "Baking", "Condiments", "Pastry", "Plant-Based", "Snacks", "Beverages"
            };

        /// The quantity value for the inventory item.
        [ObservableProperty]
        private float _quantity;

        /// The selected quantity type for the inventory item.
        [ObservableProperty]
        private string _quantityType;

        /// The selected location for the inventory item.
        [ObservableProperty]
        private string _location;

        /// The selected category for the inventory item.
        [ObservableProperty]
        private string _category;

        /// <summary>
        /// Called when the Quantity property changes. Logs the new value.
        /// </summary>
        /// <param name="value">The new quantity value.</param>
        partial void OnQuantityChanged(float value)
        {
            Debug.WriteLine($"Quantity changed: {value}");
        }

        /// <summary>
        /// Called when the QuantityType property changes. Logs the new value.
        /// </summary>
        /// <param name="value">The new quantity type.</param>
        partial void OnQuantityTypeChanged(string value)
        {
            Debug.WriteLine($"Quantity Type changed: {value}");
        }

        /// <summary>
        /// Called when the Location property changes. Logs the new value.
        /// </summary>
        /// <param name="value">The new location.</param>
        partial void OnLocationChanged(string value)
        {
            Debug.WriteLine($"Location changed: {value}");
        }

        /// <summary>
        /// Called when the Category property changes. Logs the new value.
        /// </summary>
        /// <param name="value">The new category.</param>
        partial void OnCategoryChanged(string value)
        {
            Debug.WriteLine($"Category changed: {value}");
        }

        /// <summary>
        /// Loads the inventory item with the specified ID from the service.
        /// </summary>
        /// <param name="id">The ID of the inventory item to load.</param>
        [RelayCommand]
        private async Task LoadInventory(int id)
        {
            IsBusy = true;
            try
            {
                // Loading the inventory item from the service
                Inventory = await QuicklyService.GetInventoryItem(id);
                if (Inventory == null)
                {
                    Debug.WriteLine($"No inventory item found with Id: {id}");
                    await Shell.Current.DisplayAlert("Error!", "Inventory item not found", "OK");
                }
                else
                {
                    Debug.WriteLine($"Loaded Inventory: Id={Inventory.Id}, Quantity={Inventory.Quantity}");
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error loading inventory: {ex.Message}");
                await Shell.Current.DisplayAlert("Error!", "Failed to load inventory", "OK");
            }
            finally
            {
                IsBusy = false;
            }
        }

        /// <summary>
        /// Updates the inventory item using the service. If quantity is zero, deletes the item.
        /// </summary>
        [RelayCommand]
        private async Task UpdateInventoryAsync()
        {
            IsBusy = true;
            try
            {
                Debug.WriteLine($"Updating Inventory: Id={InventoryId}, Quantity={Inventory.Quantity}, Quantity_type= {Inventory.Quantity_Type}, Location = {Inventory.Location}, Category = {Inventory.Category}");

                // Updating the inventory item
                await QuicklyService.UpdateInventory(InventoryId, Inventory.Quantity, Inventory.Quantity_Type, Inventory.Location);
                // if the quantity is zero, delete the inventory item
                if (Inventory.Quantity == 0)
                {
                    await QuicklyService.DeleteInventory(InventoryId);
                }
                else
                {
                    await Shell.Current.DisplayAlert("Success!", "Inventory updated successfully", "OK");
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error updating inventory: {ex.Message}");
                await Shell.Current.DisplayAlert("Error!", "Failed to update inventory", "OK");
            }
            finally
            {
                IsBusy = false;
                // Navigate back to the previous page after update
                await GoBackAsync();
            }
        }

        /// <summary>
        /// Deletes the inventory item using the service.
        /// </summary>
        [RelayCommand]
        private async Task DeleteInventoryAsync()
        {
            IsBusy = true;
            try
            {
                // Deleting the inventory item
                await QuicklyService.DeleteInventory(InventoryId);
                await Shell.Current.DisplayAlert("Success!", "Inventory deleted successfully", "OK");
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error deleting inventory: {ex.Message}");
                await Shell.Current.DisplayAlert("Error!", "Failed to delete inventory", "OK");
            }
            finally
            {
                IsBusy = false;
                // Navigate back to the previous page after deletion
                await GoBackAsync();
            }
        }

        /// <summary>
        /// Navigates back to the previous page. If the inventory quantity is zero, deletes the item.
        /// </summary>
        [RelayCommand]
        private async Task GoBackAsync()
        {
            if (Inventory.Quantity == 0)
            {
                await QuicklyService.DeleteInventory(InventoryId);
            }
            await Shell.Current.GoToAsync("..");
        }
    }
}