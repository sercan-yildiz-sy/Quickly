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

    protected override void OnAppearing()
    {
        base.OnAppearing();
        if (BindingContext is TryingPage2PageModel model)
        {
            if (Shell.Current.CurrentState.Location.ToString().Contains("SelectedItem="))
            {
                var selectedItemString = Shell.Current.CurrentState.Location.ToString().Split("SelectedItem=")[1];

                try
                {
                    var selectedItem = JsonSerializer.Deserialize<Inventory>(selectedItemString);
                    _model.InventoryItem = selectedItem;
                }
                catch (JsonException ex)
                {
                    // Handle deserialization error
                    Console.WriteLine($"Error deserializing Inventory: {ex.Message}");
                    // Optionally display an error message to the user
                }
            }
        }
    }
}