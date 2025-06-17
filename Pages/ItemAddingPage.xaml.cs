using System.Diagnostics;
using Quickly.Models;
using Quickly.PageModels;
namespace Quickly.Pages

{
    /// <summary>
    /// Code-behind for the item adding page.
    /// Sets up data binding to the ItemAddingPageModel and handles page lifecycle events.
    /// </summary>
    public partial class ItemAddingPage : ContentPage
    {
        // The view model for this page, providing item search, suggestions, and add-to-inventory logic.
        private readonly ItemAddingPageModel _model;

        /// <summary>
        /// Initializes a new instance of the <see cref="ItemAddingPage"/> class.
        /// Sets up data binding to the provided view model.
        /// </summary>
        /// <param name="model">The view model for the item adding page.</param>
        public ItemAddingPage(ItemAddingPageModel model)
        {
            InitializeComponent();
            BindingContext = _model = model;
        }

        /// <summary>
        /// Called when the page appears.
        /// Triggers a refresh of the available items via the view model.
        /// </summary>
        protected override async void OnAppearing()
        {
            base.OnAppearing();
            await _model.RefreshAsync();
        }
    }
}