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
        void OnEntryTextChanged(object sender, TextChangedEventArgs e)
        {
            string oldText = e.OldTextValue;
            string newText = e.NewTextValue;
            string myText = entry.Text;
        }
        void OnEntryCompleted(object sender, EventArgs e)
        {
            string text = ((Entry)sender).Text;
        }
    }
}