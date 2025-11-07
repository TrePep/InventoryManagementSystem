using Microsoft.Maui.Controls;
using System.Collections.ObjectModel;

namespace Project2._3
{
    public partial class ShopPage : ContentPage
    {
        private ObservableCollection<Item> availableItems = new ObservableCollection<Item>();
        private List<CartItem> shoppingCart;

        public ShopPage()
        {
            InitializeComponent();
            shoppingCart = new List<CartItem>();
            LoadAvailableItems();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            LoadAvailableItems(); // Refresh when page appears
        }

        private void LoadAvailableItems()
        {
            // Get items from inventory that have stock available
            var inventoryItems = InventoryManagementPage.GetInventoryList()
                .Where(item => item.GetAmountAvailable() > 0)
                .ToList();

            availableItems = new ObservableCollection<Item>(inventoryItems);
            ItemsCollectionView.ItemsSource = availableItems;

            if (availableItems.Count == 0)
            {
                DisplayAlert("No Items", "No items are currently available for purchase. Please check back later.", "OK");
            }
        }

        private async void OnAddToCartClicked(object sender, EventArgs e)
        {
            try
            {
                var button = (Button)sender;
                var item = (Item)button.CommandParameter;

                // Get available stock for this item
                int availableStock = item.GetAmountAvailable();
                
                // Check if item is already in cart and calculate remaining available quantity
                var existingCartItem = shoppingCart.FirstOrDefault(ci => ci.Item.GetId() == item.GetId());
                int remainingStock = availableStock;
                if (existingCartItem != null)
                {
                    remainingStock = availableStock - existingCartItem.Quantity;
                }

                if (remainingStock <= 0)
                {
                    await DisplayAlert("Out of Stock", 
                        $"No more units of {item.GetName()} are available. You already have the maximum quantity in your cart.", "OK");
                    return;
                }

                // Prompt for quantity with stock limit information
                string quantityPrompt = $"How many units of {item.GetName()} would you like to add?\n\n" +
                                      $"Available stock: {remainingStock} units\n" +
                                      $"Price per unit: {item.GetPrice():C}";

                string quantityInput = await DisplayPromptAsync("Add to Cart", quantityPrompt, 
                    "Add", "Cancel", "1", keyboard: Keyboard.Numeric);

                if (string.IsNullOrWhiteSpace(quantityInput))
                {
                    return; // User cancelled
                }

                if (!int.TryParse(quantityInput, out int requestedQuantity))
                {
                    await DisplayAlert("Invalid Quantity", "Please enter a valid number.", "OK");
                    return;
                }

                if (requestedQuantity <= 0)
                {
                    await DisplayAlert("Invalid Quantity", "Please enter a quantity greater than 0.", "OK");
                    return;
                }

                if (requestedQuantity > remainingStock)
                {
                    await DisplayAlert("Insufficient Stock", 
                        $"Only {remainingStock} units available. Please enter a smaller quantity.", "OK");
                    return;
                }

                // Add to cart or update existing cart item
                if (existingCartItem != null)
                {
                    existingCartItem.Quantity += requestedQuantity;
                }
                else
                {
                    shoppingCart.Add(new CartItem(item, requestedQuantity));
                }

                UpdateCartSummary();
                
                double totalCost = requestedQuantity * item.GetPrice();
                await DisplayAlert("Added to Cart", 
                    $"Added {requestedQuantity} x {item.GetName()} to your cart.\nTotal cost: {totalCost:C}", "OK");
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", $"An error occurred: {ex.Message}", "OK");
            }
        }

        private void UpdateCartSummary()
        {
            int totalItems = shoppingCart.Sum(ci => ci.Quantity);
            double totalPrice = shoppingCart.Sum(ci => ci.TotalPrice);
            CartSummaryLabel.Text = $"Cart: {totalItems} items | Total: {totalPrice:C}";
        }

        private async void OnViewCartClicked(object sender, EventArgs e)
        {
            if (shoppingCart.Count == 0)
            {
                await DisplayAlert("Empty Cart", "Your cart is empty.", "OK");
                return;
            }

            string cartDetails = "Cart Contents:\n\n";
            foreach (var cartItem in shoppingCart)
            {
                cartDetails += $"• {cartItem.DisplayText}\n";
            }
            cartDetails += $"\nTotal: {shoppingCart.Sum(ci => ci.TotalPrice):C}";

            await DisplayAlert("Shopping Cart", cartDetails, "OK");
        }

        private async void OnCheckoutClicked(object sender, EventArgs e)
        {
            if (shoppingCart.Count == 0)
            {
                await DisplayAlert("Empty Cart", "Your cart is empty. Add some items before checking out.", "OK");
                return;
            }

            string orderSummary = "Order Summary:\n\n";
            double totalAmount = 0;

            foreach (var cartItem in shoppingCart)
            {
                orderSummary += $"• {cartItem.DisplayText}\n";
                totalAmount += cartItem.TotalPrice;
            }
            orderSummary += $"\nTotal Amount: {totalAmount:C}";

            bool confirm = await DisplayAlert("Confirm Purchase", 
                $"{orderSummary}\n\nProceed with purchase?", "Yes", "No");

            if (confirm)
            {
                // Process the purchase - reduce inventory
                foreach (var cartItem in shoppingCart)
                {
                    cartItem.Item.SetAmountAvailable(cartItem.Item.GetAmountAvailable() - cartItem.Quantity);
                }

                await DisplayAlert("Purchase Complete", 
                    $"Thank you for your purchase! Total: {totalAmount:C}", "OK");

                // Clear cart and refresh items
                shoppingCart.Clear();
                UpdateCartSummary();
                LoadAvailableItems(); // Refresh to show updated stock
            }
        }

        private async void OnClearCartClicked(object sender, EventArgs e)
        {
            if (shoppingCart.Count == 0)
            {
                await DisplayAlert("Empty Cart", "Your cart is already empty.", "OK");
                return;
            }

            bool confirm = await DisplayAlert("Clear Cart", 
                "Are you sure you want to remove all items from your cart?", "Yes", "No");

            if (confirm)
            {
                shoppingCart.Clear();
                UpdateCartSummary();
                await DisplayAlert("Cart Cleared", "All items have been removed from your cart.", "OK");
            }
        }
    }
}