namespace Quicky.Pages;

public partial class TryingPage : ContentPage
{
	public TryingPage(TryingPageModel model)
	{
		InitializeComponent();
		BindingContext = model;

	}


}