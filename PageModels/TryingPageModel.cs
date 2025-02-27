using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Quicky.Models;
using Quicky.Services;


namespace Quicky.PageModels
{
    public partial class TryingPageModel : ObservableObject, IBaseClass
    {
        public ObservableCollection<Inventory> Inventory { get; set; }

        [ObservableProperty]
        ObservableCollection<Item> _items = new();

        [ObservableProperty]
        bool _isBusy;

        private readonly QuickyItemService _quickyItemService;
        [ObservableProperty]
        bool _isInventoryVisible = false;

        [ObservableProperty]
        bool _isItemsVisible = false;
        public TryingPageModel()
        {
            Inventory = new ObservableCollection<Inventory>();
            Items = new ObservableCollection<Item>();
            _quickyItemService = new QuickyItemService();

        }


        [RelayCommand]
        public async Task GetItemAsync()
        {
            if (IsBusy)
            {
                return;
            }

            try
            {
                IsBusy = true;
                var fetchedItems = await _quickyItemService.GetItems();
                Items.Clear(); 
                foreach (var item in fetchedItems)
                {
                    Items.Add(item);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Unable to get items: {ex.Message}");
                await Shell.Current.DisplayAlert("Error!", ex.Message, "OK");
            }
            finally
            {
                IsBusy = false;
            }
        }



        [RelayCommand]
        async Task AddItem()
        {
            var quickyItemService = new QuickyItemService();
            var items = await quickyItemService.GetItems();
            var name = await App.Current.MainPage.DisplayPromptAsync("Name", "Name of the Item");

            var selectedItem = items.FirstOrDefault(item => item.Name.Equals(name, StringComparison.OrdinalIgnoreCase));

            if (selectedItem == null)
            {
                await App.Current.MainPage.DisplayAlert("Not Found", "Item not found", "OK");
                return;
            }

            var id = selectedItem.Id;
            var image = selectedItem.Image;
            var category = selectedItem.Category;
            var quantityString = await App.Current.MainPage.DisplayPromptAsync("Quantity", "Enter quantity (e.g., 1.5):");
            if (!float.TryParse(quantityString, out float quantity))
            {
                await App.Current.MainPage.DisplayAlert("Invalid Input", "Please enter a valid number.", "OK");
                return;
            }

            var quantityType = await App.Current.MainPage.DisplayPromptAsync("Quantity Type", "Enter quantity type (e.g., kg, pcs):");
            var location = await App.Current.MainPage.DisplayPromptAsync("Location", "Enter storage location:");

            await QuickyService.AddInventory(id, name, quantity, quantityType, image, location, category);
            await Refresh();

        }



        [RelayCommand]
        async Task Remove(Inventory Item)
        {
            await QuickyService.DeleteInventory(Item.Id);
            await Refresh();
        }

        [RelayCommand]
        public async Task Refresh()
        {
            IsBusy = true;
            await Task.Delay(2000);
            Inventory.Clear();
            var items = await QuickyService.GetInventory();

            foreach (var item in items)
            {
                Inventory.Add(item);
            }

        }

        [RelayCommand]
        async Task ShowInventory()
        {
            IsInventoryVisible = true;
            IsItemsVisible = false;
            await Refresh(); // Load inventory from SQL
        }

        [RelayCommand]
        async Task ShowItems()
        {
            IsItemsVisible = true;
            IsInventoryVisible = false;
            await GetItemAsync(); // Load items from JSON
        }

    }
}
