using Quickly.Models;
using System.Text.Json;

namespace Quickly.Pages;

public partial class ItemDetailsPage : ContentPage
{
    private readonly ItemDetailsPageModel _viewModel;

    public ItemDetailsPage(ItemDetailsPageModel viewModel)
    {
        InitializeComponent();
        _viewModel = viewModel;
        BindingContext = _viewModel;  
    }

    protected override void OnNavigatedTo(NavigatedToEventArgs args)
    {
        base.OnNavigatedTo(args);
    }
}