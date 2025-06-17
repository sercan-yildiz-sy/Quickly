using System.Diagnostics;
using Quickly.Models;
using Quickly.PageModels;

namespace Quickly.Pages
{
    /// <summary>
    /// Code-behind for the main inventory page.
    /// Handles page lifecycle and binds the MainPageModel as the view model.
    /// </summary>
    public partial class MainPage : ContentPage
    {
        // The view model for this page, providing inventory data and commands.
        private readonly MainPageModel _model;

        /// <summary>
        /// Initializes a new instance of the <see cref="MainPage"/> class.
        /// Sets up data binding to the provided view model.
        /// </summary>
        /// <param name="model">The view model for the main page.</param>
        public MainPage(MainPageModel model)
        {
            InitializeComponent();
            BindingContext = _model = model;
        }

        /// <summary>
        /// Called when the page appears.
        /// Triggers a refresh of the inventory data via the view model.
        /// </summary>
        protected override async void OnAppearing()
        {
            base.OnAppearing();
            await _model.Refresh();
        }
    }
}