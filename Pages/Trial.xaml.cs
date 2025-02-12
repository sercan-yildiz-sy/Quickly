using System.Diagnostics;
using Quicky.Models;
using Quicky.PageModels;

namespace Quicky.Pages;

public partial class Trial: ContentPage
{
    public Trial(TrialPageModel model)
    { 
        InitializeComponent();
        BindingContext = model;
    }
}