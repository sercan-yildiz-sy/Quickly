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
        public ObservableCollection<Inventory> Inventory { get; set; } = new();

        public ObservableCollection<Item> Items { get; set; } = new();

        [ObservableProperty]
        bool _isRefreshing;

        [ObservableProperty]
        bool _isBusy;

        [ObservableProperty]
        Inventory _selectedInventoryItem;

        public TryingPageModel()
        {
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
                var fetchedItems = await QuickyItemService.GetItems().ConfigureAwait(false);
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
            var items = await QuickyItemService.GetItems().ConfigureAwait(false);
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

            await QuickyService.AddInventory(id, name, image, quantity, quantityType, location, category).ConfigureAwait(false);
            await Refresh().ConfigureAwait(false);

        }



        [RelayCommand]
        async Task DeleteItem(Inventory Item)
        {
            await QuickyService.DeleteInventory(Item.Id).ConfigureAwait(false);
            await Refresh().ConfigureAwait(false);
        }

        async Task UpdateItem(Inventory Item) {

            var quantityString = await App.Current.MainPage.DisplayPromptAsync("Quantity", "Enter Quantity");
            if (!float.TryParse(quantityString, out float quantity))
            {
                await App.Current.MainPage.DisplayAlert("Invalid Input", "Please enter a valid number.", "OK");
                return;
            }

            var quantityType = await App.Current.MainPage.DisplayPromptAsync("Quantity Type", "Enter quantity type");
            var location = await App.Current.MainPage.DisplayPromptAsync("Location", "Enter storage location:");

            await QuickyService.UpdateInventory(Item.Id, quantity, quantityType, location).ConfigureAwait(false);
            await Refresh().ConfigureAwait(false);

        }

        [RelayCommand]
        public async Task Refresh()
        {
            IsBusy = true;
            IsRefreshing = true;
            await Task.Delay(200).ConfigureAwait(false);
            Inventory.Clear();
            var items = await QuickyService.GetInventory().ConfigureAwait(false);

            foreach (var item in items)
            {
                Inventory.Add(item);
            }
            IsBusy = false;
            IsRefreshing = false;

        }


        [RelayCommand]
        private async Task GoBackAsync()
        {
            await Shell.Current.GoToAsync("..");
        }

        [RelayCommand]
        private Task GoToDetailsAsync(Inventory inventory)
            => Shell.Current.GoToAsync($"TryingPage2?id={inventory.Id}");

    }
}
