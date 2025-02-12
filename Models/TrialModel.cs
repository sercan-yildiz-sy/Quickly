using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace Quicky.Models
{
    public partial class TrialModel: ObservableObject
    {
        public ObservableCollection<Inventory> Inventory {  get; set; }

        [ObservableProperty]
        bool _isBusy;

        public TrialModel() { 
        
            Inventory = new ObservableCollection<Inventory>();

        }

        [RelayCommand]
        async Task AddItem() {
            var quickyItemService = new QuickyItemService();
            var items = await quickyItemService.GetItems();
            var name = await App.Current.MainPage.DisplayPromptAsync("Name", "Name of the Item");
            var selectedItem = items.FirstOrDefault(item => item.Name.Equals(name, StringComparison.OrdinalIgnoreCase));
            
            if (selectedItem == null) {
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
        async Task Refresh()
        {
            IsBusy= true;
            await Task.Delay(2000);
            Models.Inventory.Clear();
            var items = QuickyService.GetInventory();

            foreach (var item in items) {
                Models.Inventory.Add(item);
            }

        }

    }
}
