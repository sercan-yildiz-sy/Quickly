using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Quicky.Models;
using Quicky.Services;
using System.Diagnostics;
using System.Threading.Tasks;

namespace Quicky.PageModels
{
    [QueryProperty(nameof(InventoryId), "id")] 
    public partial class TryingPage2PageModel : ObservableObject, IBaseClass
    {
        [ObservableProperty]
        public bool _isBusy;

        [ObservableProperty]
        public Inventory _inventory;

        private int _inventoryId;
        public int InventoryId
        {
            get => _inventoryId;
            set
            {
                _inventoryId = value;
                LoadInventory(value);
            }
        }

        [RelayCommand]
        private async Task LoadInventory(int id)
        {
            IsBusy = true;
            try
            {
                Inventory = await QuickyService.GetInventoryItem(id);
                if (Inventory == null)
                {
                    Debug.WriteLine($"No inventory item found with Id: {id}");
                    await Shell.Current.DisplayAlert("Error!", "Inventory item not found", "OK");
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
        private async Task GoBackAsync()
        {
            await Shell.Current.GoToAsync("..");
        }
    }
}