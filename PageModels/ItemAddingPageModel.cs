using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Quickly.Models;
using Quickly.Services;

namespace Quickly.PageModels
{
    /// <summary>
    /// ViewModel for the item adding page. Handles searching, suggesting, and adding items to inventory
    /// </summary>
    public partial class ItemAddingPageModel : ObservableObject, IBaseClass
    {
        /// Indicates whether the view model is performing a busy operation (e.g., loading or adding items)
        [ObservableProperty]
        private bool _isBusy;

        /// The text entered in the search box to filter items
        [ObservableProperty]
        private string _searchText;

        /// A collection of suggested items based on the search text
        [ObservableProperty]
        private ObservableCollection<Item> _suggestions;

        /// The full list of items available for adding to <see cref="Inventory"/>
        public ObservableCollection<Item> Items { get; set; } = new();

        /// Indicates whether the page is currently refreshing its data
        [ObservableProperty]
        private bool _isRefreshing;


        /// <summary>
        /// Initializes a new instance of the <see cref="ItemAddingPageModel"/> class
        /// </summary>
        public ItemAddingPageModel()
        {
            Suggestions = new ObservableCollection<Item>();
        }

        /// <summary>
        /// Called when the search text changes, triggers the search command
        /// </summary>
        /// <param name="value">The new search text</param>
        partial void OnSearchTextChanged(string value)
        {
            SearchItemsCommand.Execute(null);
        }

        /// <summary>
        /// Searches for items matching the current search text and updates the suggestions list
        /// </summary>

        [RelayCommand]
        private async Task SearchItemsAsync()
        {
            // Check if the search text is at least 3 characters long before searching
            if (SearchText?.Length >= 3)
            {
                // Fetch all items from the service and filter them based on the search text
                var items = await QuicklyItemService.GetItems().ConfigureAwait(false);
                var filteredItems = items.Where(i => i.Name.Contains(SearchText, StringComparison.OrdinalIgnoreCase)).ToList();
                Suggestions = new ObservableCollection<Item>(filteredItems);
            }
            else
            {
                Suggestions.Clear();
            }
        }
        /// <summary>
        /// Refreshes the list of available items asynchronously.
        /// </summary>
        public async Task RefreshAsync()
        {
            IsBusy = true;
            IsRefreshing = true;
            await Task.Delay(200).ConfigureAwait(false);
            Items.Clear();
            // Fetch the latest items from the service
            var items = await QuicklyItemService.GetItems().ConfigureAwait(false);

            foreach (var item in items)
            {
                Items.Add(item);
            }
            IsBusy = false;
            IsRefreshing = false;
        }

        /// <summary>
        /// Adds the specified item to the inventory. If the item already exists, navigates to its details page
        /// </summary>
        /// <param name="item">The item to add to inventory.</param>
        [RelayCommand]
        public async Task AddInventoryAsync(Item item)
        {
            // Validate the item before proceeding
            if (item == null)
            {
                await MainThread.InvokeOnMainThreadAsync(async () =>
                {
                    await Shell.Current.DisplayAlert("Error!", "Item is null", "OK");
                });
                return;
            }

            try
            {
                IsBusy = true;
                // Fetch the current inventory items to check for duplicates
                var inventoryItems = await QuicklyService.GetInventory().ConfigureAwait(false);

                // Check if the item already exists in inventory
                var existingItem = inventoryItems.FirstOrDefault(i => i.Name == item.Name);
                if (existingItem != null)
                {
                    // If the item already exists in inventory, navigate to its details page
                    await MainThread.InvokeOnMainThreadAsync(async () =>
                    {
                        await GoToDetailsAsync(existingItem).ConfigureAwait(false);
                    });
                }
                else
                {
                    // Otherwise, add the new item to inventory and navigate to its details page
                    var newInventory = await QuicklyService.AddInventory(item.Id, item.Name, item.Image, 0, "kg", item.Category, "Pantry").ConfigureAwait(false);
                    await MainThread.InvokeOnMainThreadAsync(async () =>
                    {
                        await GoToDetailsAsync(newInventory).ConfigureAwait(false);
                    });
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error: {ex.Message}");
                await MainThread.InvokeOnMainThreadAsync(async () =>
                {
                    await Shell.Current.DisplayAlert("Error!", "Unable to add item", "OK");
                });
            }
            finally
            {
                IsBusy = false;
            }
        }




        /// <summary>
        /// Navigates to the details page for the specified inventory item
        /// </summary>
        /// <param name="inventory">The inventory item to view details for.</param>
        [RelayCommand]
        private Task GoToDetailsAsync(Inventory inventory)
             => Shell.Current.GoToAsync($"ItemDetailsPage?id={inventory.Id}");

    }
}