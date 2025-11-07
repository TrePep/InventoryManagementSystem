using Microsoft.Maui.Controls;

namespace Project2._3
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
        }

        private void OnInventoryManagementClicked(object sender, EventArgs e)
        {
            Navigation.PushAsync(new InventoryManagementPage()); 
        }

        private void OnShopClicked(object sender, EventArgs e)
        {
            Navigation.PushAsync(new ShopPage());
        }

        private async void OnQuitClicked(object sender, EventArgs e)
        {
            bool confirm = await DisplayAlert("Quit", "Are you sure you want to quit?", "Yes", "No");
            if (confirm)
            {
                System.Diagnostics.Process.GetCurrentProcess().CloseMainWindow();
            }
        }
    }
}
