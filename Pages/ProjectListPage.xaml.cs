using System.Diagnostics;
using Quicky.Models;
using Quicky.PageModels;
namespace Quicky.Pages

{
    public partial class ProjectListPage : ContentPage
    {
        public ProjectListPage(ProjectListPageModel model)
        {
            InitializeComponent();
            BindingContext = model;
        }
    }
}