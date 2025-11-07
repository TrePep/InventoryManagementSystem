using Microsoft.Maui.Controls;
using System.Collections.ObjectModel;

namespace Project2._3
{
    public partial class InventoryManagementPage : ContentPage
    {
        public static int IdTracker = 0;
        public static LinkedList<Item> inventory = new LinkedList<Item>();
        private ObservableCollection<Item> inventoryCollection;

        public InventoryManagementPage()
        {
            InitializeComponent();
            inventoryCollection = new ObservableCollection<Item>();
            InventoryCollectionView.ItemsSource = inventoryCollection;
            LoadInventory();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            LoadInventory(); // Refresh when page appears
        }

        private void LoadInventory()
        {
            inventoryCollection.Clear();
            foreach (var item in inventory)
            {
                System.Diagnostics.Debug.WriteLine($"Loading item: Name='{item.ItemName}', Description='{item.ItemDescription}', Price={item.ItemPrice}");
                inventoryCollection.Add(item);
            }
            UpdateInventorySummary();
            System.Diagnostics.Debug.WriteLine($"Total items in collection: {inventoryCollection.Count}");
        }

        private void UpdateInventorySummary()
        {
            int totalItems = inventoryCollection.Count;
            int totalUnits = inventoryCollection.Sum(item => item.GetAmountAvailable());
            InventorySummaryLabel.Text = $"Total Items: {totalItems} | Total Units: {totalUnits}";
        }

        private void AddItem(object sender, EventArgs e)
        {
            Navigation.PushAsync(new AddItemPage());
        }

        private async void OnEditItemClicked(object sender, EventArgs e)
        {
            try
            {
                var button = (Button)sender;
                var item = (Item)button.CommandParameter;
                
                await Navigation.PushAsync(new EditItemPage(item));
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", $"Unable to edit item: {ex.Message}", "OK");
            }
        }

        private async void OnDeleteItemClicked(object sender, EventArgs e)
        {
            try
            {
                var button = (Button)sender;
                var item = (Item)button.CommandParameter;

                string confirmationMessage = $"Are you sure you want to delete this item?\n\n" +
                    $"ID: {item.GetId()}\n" +
                    $"Name: {item.GetName()}\n" +
                    $"Description: {item.GetDescription()}\n" +
                    $"Price: {item.GetPrice():C}\n" +
                    $"Quantity: {item.GetAmountAvailable()}";

                bool confirm = await DisplayAlert("Confirm Delete", confirmationMessage, "Delete", "Cancel");

                if (confirm)
                {
                    // Remove from linked list
                    var nodeToRemove = inventory.Find(item);
                    if (nodeToRemove != null)
                    {
                        inventory.Remove(nodeToRemove);
                        
                        // Refresh the display
                        LoadInventory();
                        
                        await DisplayAlert("Success", $"Item '{item.GetName()}' has been deleted from inventory.", "OK");
                    }
                    else
                    {
                        await DisplayAlert("Error", "Item not found in inventory.", "OK");
                    }
                }
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", $"Unable to delete item: {ex.Message}", "OK");
            }
        }

        public static List<Item> GetInventoryList()
        {
            return new List<Item>(inventory);
        }

        public static void AddNewItem(Item item)
        {
            inventory.AddLast(item);
            IdTracker++;
        }

        private async void PopulateWithFakeItems(object sender, EventArgs e)
        {
            bool confirm = await DisplayAlert("Populate Inventory", 
                "This will add 10 sample items to your inventory. Continue?", "Yes", "No");
            
            if (!confirm) return;

            var fakeItems = GenerateFakeItems();
            int addedCount = 0;

            foreach (var item in fakeItems)
            {
                AddNewItem(item);
                addedCount++;
            }

            LoadInventory(); // Refresh the UI after adding items

            await DisplayAlert("Success", 
                $"Added {addedCount} sample items to the inventory!", "OK");
        }

        private static List<Item> GenerateFakeItems()
        {
            var fakeItems = new List<Item>
            {
                new Item("Laptop", "High-performance gaming laptop with RGB keyboard", 1299.99, IdTracker, 15),
                new Item("Wireless Mouse", "Ergonomic wireless mouse with precision tracking", 29.99, IdTracker + 1, 50),
                new Item("Mechanical Keyboard", "Cherry MX Blue switches, RGB backlit", 89.99, IdTracker + 2, 25),
                new Item("4K Monitor", "27-inch 4K UHD monitor with HDR support", 399.99, IdTracker + 3, 8),
                new Item("Smartphone", "Latest flagship phone with 128GB storage", 799.99, IdTracker + 4, 20),
                new Item("Bluetooth Headphones", "Noise-cancelling over-ear headphones", 149.99, IdTracker + 5, 30),
                new Item("USB-C Cable", "High-speed USB-C charging and data cable", 12.99, IdTracker + 6, 100),
                new Item("Portable SSD", "1TB portable solid state drive", 179.99, IdTracker + 7, 12),
                new Item("Webcam", "1080p HD webcam with auto-focus", 59.99, IdTracker + 8, 35),
                new Item("Desk Lamp", "LED desk lamp with adjustable brightness", 34.99, IdTracker + 9, 18),
                new Item("Coffee Mug", "Programmers fuel - 16oz ceramic mug", 14.99, IdTracker + 10, 75),
                new Item("Power Bank", "20000mAh portable charger with fast charging", 49.99, IdTracker + 11, 22),
                new Item("Tablet", "10-inch tablet perfect for reading and browsing", 249.99, IdTracker + 12, 16),
                new Item("Wireless Charger", "Qi-compatible fast wireless charging pad", 24.99, IdTracker + 13, 40),
                new Item("Gaming Chair", "Ergonomic gaming chair with lumbar support", 199.99, IdTracker + 14, 6)
            };

            return fakeItems;
        }
    }
}
