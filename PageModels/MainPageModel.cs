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
    /// <summary>
    /// ViewModel for the main inventory page.
    /// Handles inventory filtering, location/category selection, and navigation.
    /// </summary>
    public partial class MainPageModel : ObservableObject, IBaseClass
    {
        /// The collection of inventory items currently displayed.
        public ObservableCollection<Inventory> Inventory { get; set; } = new();

        /// Indicates whether the page is currently refreshing its data.
        [ObservableProperty]
        bool _isRefreshing;

        /// Indicates whether the view model is performing a busy operation (e.g., loading or filtering).
        [ObservableProperty]
        bool _isBusy;

        /// The currently selected inventory item in the UI.
        [ObservableProperty]
        Inventory _selectedInventoryItem;

        /// List of inventory categories for filtering.
        [ObservableProperty]
        private List<string> _categories = new List<string>
            {
                "All Items", "Produce", "Meat", "Dairy", "Canned Products", "Dry Products", "Frozen Products",
                "Seafood", "Baking", "Condiments", "Pastry", "Plant-Based", "Snacks", "Beverages"
            };

        /// <summary>
        /// Initializes a new instance of the <see cref="MainPageModel"/> class.
        /// </summary>
        public MainPageModel()
        {
            Debug.WriteLine("MainPageModel instantiated.");
        }

        /// The currently selected category for filtering inventory.
        [ObservableProperty]
        private string category = "All Items";

        // The currently selected location for filtering inventory ("All", "Pantry", "Fridge", "Freezer").
        private string location = "All";

        /// <summary>
        /// Called when the Category property changes. Triggers inventory filtering.
        /// </summary>
        /// <param name="value">The new category value.</param>
        partial void OnCategoryChanged(string value)
        {
            Debug.WriteLine($"Category changed: {value}");
            category = value;
            Debug.WriteLine("Calling CurrentInventory from OnCategoryChanged...");
            CurrentInventory(category);
        }

        /// <summary>
        /// Loads and filters the inventory based on the selected category and location.
        /// </summary>
        /// <param name="category">The category to filter by.</param>
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
                // Fetch inventory items from the service based on the current location.
                var items = await QuicklyService.GetInventory(location);
                Debug.WriteLine($"Fetched {items?.Count() ?? 0} items from service.");

                // Filter items based on all categories and quantity > 0.
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
                // If a specific category is selected, filter by that category and quantity > 0.
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

        /// Gets the color for the Pantry button based on the current location filter.
        public Color PantryButtonColor => location == "Pantry" ? Color.FromArgb("#40C0372F") : Colors.Transparent;

        /// Gets the color for the Fridge button based on the current location filter.
        public Color FridgeButtonColor => location == "Fridge" ? Color.FromArgb("#40C0372F") : Colors.Transparent;

        /// Gets the color for the Freezer button based on the current location filter.
        public Color FreezerButtonColor => location == "Freezer" ? Color.FromArgb("#40C0372F") : Colors.Transparent;

        /// <summary>
        /// Handles the Pantry button click. Toggles the location filter and refreshes inventory.
        /// </summary>
        [RelayCommand]
        public async Task PantryItemsAsync()
        {
            Debug.WriteLine("PantryItemsAsync called.");
            if (IsBusy)
            {
                Debug.WriteLine("PantryItemsAsync aborted: IsBusy is true.");
                return;
            }
            // Set the location to "Pantry" or "All" based on current state and change the color of buttons.
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

        /// <summary>
        /// Handles the Fridge button click. Toggles the location filter and refreshes inventory.
        /// </summary>
        [RelayCommand]
        public async Task FridgeItemsAsync()
        {
            Debug.WriteLine("FridgeItemsAsync called.");
            if (IsBusy)
            {
                Debug.WriteLine("FridgeItemsAsync aborted: IsBusy is true.");
                return;
            }
            // Set the location to "Fridge" or "All" based on current state and change the color of buttons.
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

        /// <summary>
        /// Handles the Freezer button click. Toggles the location filter and refreshes inventory.
        /// </summary>
        [RelayCommand]
        public async Task FreezerItemsAsync()
        {
            Debug.WriteLine("FreezerItemsAsync called.");
            if (IsBusy)
            {
                Debug.WriteLine("FreezerItemsAsync aborted: IsBusy is true.");
                return;
            }
            // Set the location to "Freezer" or "All" based on current state and change the color of buttons.
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

        /// <summary>
        /// Refreshes the inventory list from the service.
        /// </summary>
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
                // Fetch the inventory items from the service based on the current location.
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

        /// <summary>
        /// Navigates back to the previous page.
        /// </summary>
        [RelayCommand]
        private async Task GoBackAsync()
        {
            Debug.WriteLine("GoBackAsync called.");
            await Shell.Current.GoToAsync("..");
        }

        /// <summary>
        /// Navigates to the details page for the specified inventory item.
        /// </summary>
        /// <param name="inventory">The inventory item to view details for.</param>
        [RelayCommand]
        private Task GoToDetailsAsync(Inventory inventory)
        {
            Debug.WriteLine($"GoToDetailsAsync called for Inventory Id: {inventory?.Id}");
            return Shell.Current.GoToAsync($"ItemDetailsPage?id={inventory.Id}");
        }
    }
}
