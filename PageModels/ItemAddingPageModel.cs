#nullable disable
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
    public partial class ItemAddingPageModel : ObservableObject, IBaseClass
    {
        [ObservableProperty]
        private bool _isBusy;

        [ObservableProperty]
        private string _searchText;

        [ObservableProperty]
        private ObservableCollection<Item> _suggestions;

        public ObservableCollection<Item> Items { get; set; } = new();

        [ObservableProperty]
        private bool _isRefreshing;

        public ItemAddingPageModel()
        {
            Suggestions = new ObservableCollection<Item>();
        }

        partial void OnSearchTextChanged(string value)
        {
            SearchItemsCommand.Execute(null);
        }

        [RelayCommand]
        private async Task SearchItemsAsync()
        {
            if (SearchText?.Length >= 3)
            {
                var items = await QuicklyItemService.GetItems().ConfigureAwait(false);
                var filteredItems = items.Where(i => i.Name.Contains(SearchText, StringComparison.OrdinalIgnoreCase)).ToList();
                Suggestions = new ObservableCollection<Item>(filteredItems);
            }
            else
            {
                Suggestions.Clear();
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }


        public async Task RefreshAsync()
        {
            IsBusy = true;
            IsRefreshing = true;
            await Task.Delay(200).ConfigureAwait(false);
            Items.Clear();
            var items = await QuicklyItemService.GetItems().ConfigureAwait(false);

            foreach (var item in items)
            {
                Items.Add(item);
            }
            IsBusy = false;
            IsRefreshing = false;
        }

        [RelayCommand]
        public async Task AddInventoryAsync(Item item)
        {
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

                var inventoryItems = await QuicklyService.GetInventory().ConfigureAwait(false);
                

                var existingItem = inventoryItems.FirstOrDefault(i => i.Name == item.Name);
                if (existingItem != null)
                {
                    await MainThread.InvokeOnMainThreadAsync(async () =>
                    {
                        await GoToDetailsAsync(existingItem).ConfigureAwait(false);
                    });
                }
                else
                {
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





        [RelayCommand]
        private Task GoToDetailsAsync(Inventory inventory)
             => Shell.Current.GoToAsync($"ItemDetailsPage?id={inventory.Id}");

    }
}