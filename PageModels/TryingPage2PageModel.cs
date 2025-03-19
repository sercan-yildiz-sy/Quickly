using CommunityToolkit.Mvvm.ComponentModel;
using Quicky.Models;
using Quicky.Services;
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

        private async Task LoadInventory(int id)
        {
            IsBusy = true;
            try
            {
                Inventory = await QuickyService.GetInventoryItem(id);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error loading inventory: {ex.Message}");
            }
            finally
            {
                IsBusy = false;
            }
        }
    }
}