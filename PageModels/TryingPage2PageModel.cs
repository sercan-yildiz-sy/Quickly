using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Quickly.Models;
using Quickly.Services;
using System.Diagnostics;
using System.Threading.Tasks;

namespace Quickly.PageModels
{
    [QueryProperty(nameof(InventoryId), "id")] 
    public partial class TryingPage2PageModel : ObservableObject, IBaseClass
    {
        [ObservableProperty]
        public bool _isBusy;

        [ObservableProperty]
        public Inventory _inventory = new Inventory();

        private int _inventoryId;
        public int InventoryId
        {
            get => _inventoryId;
            set
            {
                _inventoryId = value;
                _ = LoadInventory(value);
            }
        }
        [ObservableProperty]
        private List<string> _quantityTypes = new List<string> { "Unit", "kg", "lb", "lt", "oz" };

        [ObservableProperty]
        private List<string> _locations = new List<string> { "Pantry", "Fridge", "Freezer" };

        [ObservableProperty]
        private List<string> _categories = new List<string> { "All Items", "Produce", "Meat", "Dry Food", "Canned Food", "Others" };


        [ObservableProperty]
        private float _quantity;

        [ObservableProperty]
        private string _quantityType;

        [ObservableProperty]
        private string _location;

        [ObservableProperty]
        private string _category;

        partial void OnQuantityChanged(float value)
        {
            Debug.WriteLine($"Quantity changed: {value}");
        }

        partial void OnQuantityTypeChanged(string value)
        {
            Debug.WriteLine($"Quantity Type changed: {value}");
        }
        partial void OnLocationChanged(string value)
        {
            Debug.WriteLine($"Location changed: {value}");
        }
        partial void OnCategoryChanged(string value)
        {
            Debug.WriteLine($"Category changed: {value}");
        }

        [RelayCommand]
        private async Task LoadInventory(int id)
        {
            IsBusy = true;
            try
            {
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




        [RelayCommand]
        private async Task UpdateInventoryAsync()
        {
            IsBusy = true;
            try
            {
                Debug.WriteLine($"Updating Inventory: Id={InventoryId}, Quantity={Inventory.Quantity}, Quantity_type= {Inventory.Quantity_Type}, Location = {Inventory.Location}, Category = {Inventory.Category}");
                await QuicklyService.UpdateInventory(InventoryId, Inventory.Quantity, Inventory.Quantity_Type, Inventory.Location);
                await Shell.Current.DisplayAlert("Success!", "Inventory updated successfully", "OK");
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error updating inventory: {ex.Message}");
                await Shell.Current.DisplayAlert("Error!", "Failed to update inventory", "OK");
            }
            finally
            {
                IsBusy = false;
                await GoBackAsync();
            }
        }

        [RelayCommand]
        private async Task DeleteInventoryAsync()
        {
            IsBusy = true;
            try
            {
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
                await GoBackAsync();
            }
        }


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