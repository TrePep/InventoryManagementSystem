using Microsoft.Maui.Controls;
using System.Collections.Generic;

namespace Project2._3
{
    public partial class ViewInventoryPage : ContentPage
    {
        public ViewInventoryPage()
        {
            InitializeComponent();

            // Use Items directly now that they have proper binding properties
            var inventoryList = InventoryManagementPage.GetInventoryList();
            InventoryCollectionView.ItemsSource = inventoryList;
        }
    }
}
