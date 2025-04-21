#nullable disable
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Quicky.Data;
using Quicky.Models;
using Quicky.Services;

namespace Quicky.PageModels
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
                var items = await QuickyItemService.GetItems().ConfigureAwait(false);
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

        [RelayCommand]
        public async Task AddItemAsync()
        {
            if (IsBusy)
            {
                return;
            }
            try
            {
                IsBusy = true;
                await QuickyItemService.GetItems().ConfigureAwait(false);
            }
            catch
            {
                await Shell.Current.DisplayAlert("Error!", "Unable to get items", "OK");
            }
            finally
            {
                IsBusy = false;
            }
        }

        public async Task RefreshAsync()
        {
            IsBusy = true;
            IsRefreshing = true;
            await Task.Delay(200).ConfigureAwait(false);
            Items.Clear();
            var items = await QuickyItemService.GetItems().ConfigureAwait(false);

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
                await Shell.Current.DisplayAlert("Error!", "Item is null", "OK");
                return;
            }

            try
            {
                IsBusy = true;

                var inventoryItems = await QuickyService.GetInventory().ConfigureAwait(false);
                var existingItem = inventoryItems.FirstOrDefault(i => i.Name == item.Name);
                if (existingItem != null)
                {
                    
                    await GoToDetailsAsync(existingItem).ConfigureAwait(true);
                }
                else
                {
                    var newInventory = await QuickyService.AddInventory(item.Id, item.Name, item.Image, 0, "kg", item.Category, "All").ConfigureAwait(false);
                    await GoToDetailsAsync(newInventory).ConfigureAwait(true);
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error: {ex.Message}");
                await Shell.Current.DisplayAlert("Error!", "Unable to add item", "OK");
            }
            finally
            {
                IsBusy = false;
            }
        }
        [RelayCommand]
        private Task GoToDetailsAsync(Inventory inventory)
            => Shell.Current.GoToAsync($"TryingPage2?id={2}");
    }
}