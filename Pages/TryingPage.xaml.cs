using Microsoft.Maui.Controls;
using Quicky.PageModels;

namespace Quicky.Pages;

public partial class TryingPage : ContentPage
{
    private readonly TryingPageModel _model;

    public TryingPage(TryingPageModel model)
    {
        InitializeComponent();
        BindingContext = _model = model;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        await _model.GetItemAsync(); 
    }
}