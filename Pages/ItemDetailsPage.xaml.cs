using Quickly.Models;
using System.Text.Json;

namespace Quickly.Pages;

/// <summary>
/// Code-behind for the item details page.
/// Sets up data binding to the ItemDetailsPageModel for displaying and editing inventory item details.
/// </summary>
public partial class ItemDetailsPage : ContentPage
{
    /// <summary>
    /// Initializes a new instance of the <see cref="ItemDetailsPage"/> class.
    /// Sets the BindingContext to the provided view model for data binding.
    /// </summary>
    /// <param name="model">The view model for the item details page.</param>
    public ItemDetailsPage(ItemDetailsPageModel model)
    {
        InitializeComponent();
        BindingContext = model;
    }
}
