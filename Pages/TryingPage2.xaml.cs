namespace Quicky.Pages;

public partial class TryingPage2 : ContentPage
{
	public TryingPage2(TryingPage2PageModel model)
	{
		InitializeComponent();
		BindingContext = model;+
	}
}