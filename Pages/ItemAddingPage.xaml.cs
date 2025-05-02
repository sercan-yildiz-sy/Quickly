using System.Diagnostics;
using Quickly.Models;
using Quickly.PageModels;
namespace Quickly.Pages

{
    public partial class ItemAddingPage : ContentPage
    {
        private readonly ItemAddingPageModel _model;
        public ItemAddingPage(ItemAddingPageModel model)
        {
            InitializeComponent();
            BindingContext = _model = model;
        }
    
        protected override async void OnAppearing()
        {
            base.OnAppearing();
            await _model.RefreshAsync();
        }
    }
}