using Microsoft.Maui.Controls;
using Quicky.Models;
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
        await _model.Refresh(); 
    }
    private void CollectionView_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        if (e.CurrentSelection != null && e.CurrentSelection.Count > 0)
        {
            _model.SelectionChangedCommand((Inventory)e.CurrentSelection.FirstOrDefault());
            // Remove this line: ((CollectionView)sender).SelectedItem = null;
        }
    }
}