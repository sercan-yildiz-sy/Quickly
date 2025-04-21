using System.Diagnostics;
using Quicky.Models;
using Quicky.PageModels;
namespace Quicky.Pages

{
    public partial class ItemAddingPage : ContentPage
    {
        private readonly ItemAddingPageModel _model;
        public ItemAddingPage(ItemAddingPageModel model)
        {
            InitializeComponent();
            BindingContext = _model = model;
        }
        void OnEntryTextChanged(object sender, TextChangedEventArgs e)
        {
            string oldText = e.OldTextValue;
            string newText = e.NewTextValue;
            string myText = Entry.Text;
        }
        void OnEntryCompleted(object sender, EventArgs e)
        {
            string text = ((Entry)sender).Text;
        }
        
        protected override async void OnAppearing()
        {
            base.OnAppearing();
            await _model.RefreshAsync();
        }
    }
}