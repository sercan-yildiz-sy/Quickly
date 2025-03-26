#nullable disable
using System.Collections.ObjectModel;
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
        bool _isBusy;

        public ObservableCollection<Item> Items{ get; set; } = new();

        [ObservableProperty]
        bool _isRefreshing;

        public ItemAddingPageModel()
        {
        }

        [RelayCommand]
        public async Task AdditemAsync()
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


        public async Task Refresh()
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
        public async Task AddInventoryAsync()
        {
            await Shell.Current.GoToAsync($"TryingPage2");
        }
    }
}