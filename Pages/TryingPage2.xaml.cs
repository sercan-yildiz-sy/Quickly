using Quickly.Models;
using System.Text.Json;

namespace Quickly.Pages;

public partial class TryingPage2 : ContentPage
{
    private readonly TryingPage2PageModel _viewModel;

    public TryingPage2(TryingPage2PageModel viewModel)
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