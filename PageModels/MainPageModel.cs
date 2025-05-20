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
    public partial class MainPageModel : ObservableObject, IBaseClass
    {
        public ObservableCollection<Inventory> Inventory { get; set; } = new();

        public ObservableCollection<Item> Items { get; set; } = new();

        [ObservableProperty]
        bool _isRefreshing;

        [ObservableProperty]
        bool _isBusy;

        [ObservableProperty]
        Inventory _selectedInventoryItem;
        
        [ObservableProperty]
        private List<string> _categories = new List<string> { "All Items", "Produce", "Meat", "Dry Food", "Canned Food", "Others" };


        public MainPageModel()
        {
        }

        [ObservableProperty]
        private string category = "All Items";

        private string location = "All";
        partial void OnCategoryChanged(string value)
        {
            Debug.WriteLine($"Category changed: {value}");
            category = value;
            CurrentInventory(category);
        }


        private async Task CurrentInventory(string category)
        {
            if (IsBusy)
            {
                return;
            }

            try
            {
                IsBusy = true;
                var items = await QuicklyService.GetInventory(location).ConfigureAwait(false);
                Inventory.Clear();
                if (category == "All Items" || String.IsNullOrEmpty(category))
                {
                    foreach (var item in items)
                    {
                        if (item.Quantity > 0)
                        {
                            Inventory.Add(item);
                        }
                    }
                }
                else
                {
                    foreach (var item in items)
                    {
                        if (item.Category == category)
                        {
                            if (item.Quantity > 0)
                                Inventory.Add(item);
                        }
                    }
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
        public async Task PantryItemsAsync()
        {
            if (IsBusy)
            {
                return;
            }
            try
            {
                location = "Pantry";
                await CurrentInventory(category).ConfigureAwait(false);
               
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
                location = "Fridge";
                await CurrentInventory(category).ConfigureAwait(false);
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
                location = "Freezer";
                await CurrentInventory(category).ConfigureAwait(false);
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
            Debug.WriteLine("Refreshing inventory...");
            IsBusy = true;
            IsRefreshing = true;
            

            try
            {
                var items = await QuicklyService.GetInventory(location).ConfigureAwait(false);

                Inventory.Clear();
                foreach (var item in items)
                {
                    if (item.Quantity > 0)
                        Inventory.Add(item);
                }

                Debug.WriteLine($"Inventory refreshed. Total items: {Inventory.Count}");
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error refreshing inventory: {ex.Message}");
            }
            finally
            {
                IsBusy = false;
                IsRefreshing = false;
            }
        }



        [RelayCommand]
        private async Task GoBackAsync()
        {
            await Shell.Current.GoToAsync("..");
        }

        [RelayCommand]
        private Task GoToDetailsAsync(Inventory inventory)
            => Shell.Current.GoToAsync($"ItemDetailsPage?id={inventory.Id}");

    }
}
