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
using Quickly.Models;
using Quickly.Services;


namespace Quickly.PageModels
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

        [ObservableProperty]
        private string _category;

        partial void OnCategoryChanged(string value)
        {
            Debug.WriteLine($"Category changed: {value}");
            UpdateInventoryByCategory(value);
        }

        private async Task UpdateInventoryByCategory(string category)
        {
            if (string.IsNullOrEmpty(category) || category == "All Items")
            {

                await Refresh().ConfigureAwait(false);
            }

            else
            {
                try
                {
                    IsBusy = true;
                    
                }
            }
        }




        [RelayCommand]
        public async Task PantryItemsAsync()
        {
            if (IsBusy)
            {
                return;
            }
            try
            {
                IsBusy = true;
                var items = await QuicklyService.GetInventory("Pantry").ConfigureAwait(false);
                Inventory.Clear();
                foreach (var item in items)
                {
                    Inventory.Add(item);
                }
               
            }

            catch (Exception ex)
            {
                Debug.WriteLine($"Error loading inventory: {ex.Message}");
            }
            finally
            {
                IsBusy = false;
            }
        }

        [RelayCommand]
        public async Task FridgeItemsAsync()
        {
            if (IsBusy)
            {
                return;
            }
            try
            {
                IsBusy = true;
                var items = await QuicklyService.GetInventory("Fridge").ConfigureAwait(false);
                Inventory.Clear();
                foreach (var item in items)
                {
                    Inventory.Add(item);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error loading inventory: {ex.Message}");
            }
            finally
            {
                IsBusy = false;
            }
        }

        [RelayCommand]
        public async Task FreezerItemsAsync()
        {
            if (IsBusy)
            {
                return;
            }
            try
            {
                IsBusy = true;
                var items = await QuicklyService.GetInventory("Freezer").ConfigureAwait(false);
                Inventory.Clear();
                foreach (var item in items)
                {
                    Inventory.Add(item);
                }
            }
            catch (Exception ex) 
            {
                Debug.WriteLine($"Error loading inventory: {ex.Message}");
            }
            finally
            {
                IsBusy = false;
            }

        }

        [RelayCommand]
        public async Task Refresh()
        {
            IsBusy = true;
            IsRefreshing = true;
            await Task.Delay(200).ConfigureAwait(false);
            Inventory.Clear();
            var items = await QuicklyService.GetInventory().ConfigureAwait(false);

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
