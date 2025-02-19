using System.Diagnostics;
using Quicky.Models;
using Quicky.PageModels;

namespace Quicky.Pages
{
    public partial class MainPage : ContentPage
    {
        public MainPage(MainPageModel model)
        {
            InitializeComponent();
            BindingContext = model;
        }
        private async void GoToTryingPage_Clicked(object sender, EventArgs e)
        {
            await Shell.Current.GoToAsync(($"//TryingPage"));
        }
    }
}