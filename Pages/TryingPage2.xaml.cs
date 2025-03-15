using Quicky.Models;
using System.Text.Json;

namespace Quicky.Pages;

public partial class TryingPage2 : ContentPage
{
    private readonly TryingPage2PageModel _model;
    public TryingPage2(TryingPage2PageModel model)
    {
        InitializeComponent();
        BindingContext = _model = model;
    }

    protected override void OnNavigatedTo(NavigatedToEventArgs args)
    {
        base.OnNavigatedTo(args);
    }
}