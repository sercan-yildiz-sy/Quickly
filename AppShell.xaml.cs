namespace Quickly
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();
            Routing.RegisterRoute("ItemDetailsPage", typeof(ItemDetailsPage));
            this.Navigated += OnShellNavigated;
        }

        private void OnShellNavigated(object sender, ShellNavigatedEventArgs e)
        {
            var mainTab = this.FindByName<ShellContent>("MainShellContent");
            var addTab = this.FindByName<ShellContent>("AddShellContent");
            var profileTab = this.FindByName<ShellContent>("ProfileShellContent");

            if (mainTab != null) mainTab.Icon = "main_page.png";
            if (addTab != null) addTab.Icon = "add_page.png";
            if (profileTab != null) profileTab.Icon = "profile_page.png";

            var currentSection = this.CurrentItem?.CurrentItem;
            var currentContent = currentSection?.CurrentItem as ShellContent;

            if (currentContent == mainTab)
                mainTab.Icon = "main_page_active.png";
            else if (currentContent == addTab)
                addTab.Icon = "add_page_active.png";
            else if (currentContent == profileTab)
                profileTab.Icon = "profile_page_active.png";
        }
        

    }
}
