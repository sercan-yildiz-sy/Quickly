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
        private List<string> _categories = new List<string>{"All Items", "Produce", "Meat", "Dairy", "Canned Products", "Dry Products", "Frozen Products", "Seafood", "Baking", "Condiments", "Pastry", "Plant-Based", "Snacks", "Beverages"};

        public MainPageModel()
        {
            Debug.WriteLine("MainPageModel instantiated.");
        }

        [ObservableProperty]
        private string category = "All Items";

        private string location = "All";

        partial void OnCategoryChanged(string value)
        {
            Debug.WriteLine($"Category changed: {value}");
            category = value;
            Debug.WriteLine("Calling CurrentInventory from OnCategoryChanged...");
            CurrentInventory(category);
        }

        [RelayCommand]
        private async Task CurrentInventory(string category)
        {
            Debug.WriteLine($"CurrentInventory called with category: {category}, location: {location}");
            if (IsBusy)
            {
                Debug.WriteLine("CurrentInventory aborted: IsBusy is true.");
                return;
            }

            try
            {
                IsBusy = true;
                Debug.WriteLine("Fetching inventory from service...");
                var items = await QuicklyService.GetInventory(location);
                Debug.WriteLine($"Fetched {items?.Count() ?? 0} items from service.");

                if (category == "All Items" || String.IsNullOrEmpty(category))
                {
                    Debug.WriteLine("Filtering for all items with quantity > 0.");
                    await MainThread.InvokeOnMainThreadAsync(() =>
                    {
                        Inventory.Clear();
                        foreach (var item in items)
                        {
                            if (item.Quantity > 0)
                                Inventory.Add(item);
                        }
                        Debug.WriteLine($"Inventory updated. Count: {Inventory.Count}");
                    });
                }
                else
                {
                    Debug.WriteLine($"Filtering for category: {category} with quantity > 0.");
                    await MainThread.InvokeOnMainThreadAsync(() =>
                    {
                        Inventory.Clear();
                        foreach (var item in items)
                        {
                            if (item.Quantity > 0 && item.Category == category)
                                Inventory.Add(item);
                        }
                        Debug.WriteLine($"Inventory updated. Count: {Inventory.Count}");
                    });
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error loading inventory: {ex.Message}");
            }
            finally
            {
                IsBusy = false;
                Debug.WriteLine("CurrentInventory finished.");
            }
        }

        public Color PantryButtonColor => location == "Pantry" ? Color.FromArgb("#40C0372F") : Colors.Transparent;
        public Color FridgeButtonColor => location == "Fridge" ? Color.FromArgb("#40C0372F") : Colors.Transparent;
        public Color FreezerButtonColor => location == "Freezer" ? Color.FromArgb("#40C0372F") : Colors.Transparent;

        [RelayCommand]
        public async Task PantryItemsAsync()
        {
            Debug.WriteLine("PantryItemsAsync called.");
            if (IsBusy)
            {
                Debug.WriteLine("PantryItemsAsync aborted: IsBusy is true.");
                return;
            }

            try
            {
                location = location == "Pantry" ? "All" : "Pantry";
                Debug.WriteLine($"PantryItemsAsync set location to: {location}");
                await CurrentInventory(category);
                OnPropertyChanged(nameof(PantryButtonColor));
                OnPropertyChanged(nameof(FridgeButtonColor));
                OnPropertyChanged(nameof(FreezerButtonColor));
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error loading inventory: {ex.Message}");
            }
        }

        [RelayCommand]
        public async Task FridgeItemsAsync()
        {
            Debug.WriteLine("FridgeItemsAsync called.");
            if (IsBusy)
            {
                Debug.WriteLine("FridgeItemsAsync aborted: IsBusy is true.");
                return;
            }

            try
            {
                location = location == "Fridge" ? "All" : "Fridge";
                Debug.WriteLine($"FridgeItemsAsync set location to: {location}");
                await CurrentInventory(category);
                OnPropertyChanged(nameof(PantryButtonColor));
                OnPropertyChanged(nameof(FridgeButtonColor));
                OnPropertyChanged(nameof(FreezerButtonColor));
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error loading inventory: {ex.Message}");
            }
        }

        [RelayCommand]
        public async Task FreezerItemsAsync()
        {
            Debug.WriteLine("FreezerItemsAsync called.");
            if (IsBusy)
            {
                Debug.WriteLine("FreezerItemsAsync aborted: IsBusy is true.");
                return;
            }

            try
            {
                location = location == "Freezer" ? "All" : "Freezer";
                Debug.WriteLine($"FreezerItemsAsync set location to: {location}");
                await CurrentInventory(category);
                OnPropertyChanged(nameof(PantryButtonColor));
                OnPropertyChanged(nameof(FridgeButtonColor));
                OnPropertyChanged(nameof(FreezerButtonColor));
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error loading inventory: {ex.Message}");
            }
        }

        [RelayCommand]
        public async Task Refresh()
        {
            if (IsBusy)
            {
                Debug.WriteLine("Refresh aborted: IsBusy is true.");
                return;
            }
            Debug.WriteLine("Refresh called.");
            IsBusy = true;
            IsRefreshing = true;

            try
            {
                Debug.WriteLine("Fetching inventory from service (Refresh)...");
                var items = await QuicklyService.GetInventory(location);

                await MainThread.InvokeOnMainThreadAsync(() =>
                {
                    Inventory.Clear();
                    foreach (var item in items)
                    {
                        if (item.Quantity > 0)
                            Inventory.Add(item);
                    }
                    Debug.WriteLine($"Inventory refreshed. Count: {Inventory.Count}");
                });
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error refreshing inventory: {ex.Message}");
            }
            finally
            {
                IsBusy = false;
                IsRefreshing = false;
                Debug.WriteLine("Refresh finished.");
            }
        }

        [RelayCommand]
        private async Task GoBackAsync()
        {
            Debug.WriteLine("GoBackAsync called.");
            await Shell.Current.GoToAsync("..");
        }

        [RelayCommand]
        private Task GoToDetailsAsync(Inventory inventory)
        {
            Debug.WriteLine($"GoToDetailsAsync called for Inventory Id: {inventory?.Id}");
            return Shell.Current.GoToAsync($"ItemDetailsPage?id={inventory.Id}");
        }
    }
}
