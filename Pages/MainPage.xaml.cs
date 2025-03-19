using System.Diagnostics;
using Quicky.Models;
using Quicky.PageModels;

namespace Quicky.Pages
{
    public partial class MainPage : ContentPage
    {
        private readonly TryingPageModel _model;

        public MainPage(TryingPageModel model)
        {
            InitializeComponent();
            BindingContext = _model = model;
        }
        private async void GoToTryingPage_Clicked(object sender, EventArgs e)
        {
            await Shell.Current.GoToAsync(($"//TryingPage"));
        }
        protected override async void OnAppearing()
        {
            base.OnAppearing();
            await _model.Refresh();
        }
    }
}