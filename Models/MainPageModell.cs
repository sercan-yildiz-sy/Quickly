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
    public partial class MainPageModell: ObservableObject
    {
        public ObservableCollection<Inventory> Inventory {  get; set; }

        [ObservableProperty]
        bool _isBusy;

        public MainPageModell() { 
        
            Inventory = new ObservableCollection<Inventory>();

        }

        [RelayCommand]
        async Task AddItem() {
            var Id = await App.Current.MainPage.DisplayPromptAsync("Id", "Id of item");
            var Name = await App.Current.MainPage.DisplayPromptAsync("Name", "Name of item");
            var quantityString = await App.Current.MainPage.DisplayPromptAsync("Quantity", "Enter quantity (e.g., 1.5):");
            if (float.TryParse(quantityString, out float Quantity))
            {
            }
            else
            {
                await App.Current.MainPage.DisplayAlert("Invalid Input", "Please enter a valid number.", "OK");
            }
            var Quantity_Type = await App.Current.MainPage.DisplayPromptAsync("Roaster", "Roaster of coffee");
            var Image = await App.Current.MainPage.DisplayPromptAsync("Roaster", "Roaster of coffee");
            var Location = await App.Current.MainPage.DisplayPromptAsync("Roaster", "Roaster of coffee");

            await QuickyService.AddItem(Id, Name, Quantity, Quantity_Type, Image, Location);
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
