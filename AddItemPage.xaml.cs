using Microsoft.Maui.Controls;

namespace Project2._3
{
    public partial class AddItemPage : ContentPage
    {
        public AddItemPage()
        {
            InitializeComponent();
        }

        private async void OnSubmitClicked(object sender, EventArgs e)
        {
            string name = NameEntry.Text;
            string description = DescriptionEntry.Text;
            double price = double.TryParse(PriceEntry.Text, out double parsedPrice) ? parsedPrice : 0.0;
            int amountAvailable = int.TryParse(AmountEntry.Text, out int parsedAmount) ? parsedAmount : 0;

            int currentId = InventoryManagementPage.IdTracker;
            string confirmationMessage = $"ID: {currentId}\nName: {name}\nDescription: {description}\nPrice: {price:C}\nAmount Available: {amountAvailable}";

            bool answer = await DisplayAlert("Are you sure?", confirmationMessage, "Yes", "No");

            if (answer)
            {
                Item newItem = new Item(name, description, price, currentId, amountAvailable);
                InventoryManagementPage.AddNewItem(newItem);

                await DisplayAlert("Success", $"Item ID {currentId} has been added to the inventory.", "OK");
                NameEntry.Text = string.Empty;
                DescriptionEntry.Text = string.Empty;
                PriceEntry.Text = string.Empty;
                AmountEntry.Text = string.Empty;
            }
            else
            {
                await DisplayAlert("Cancelled", "Item addition has been cancelled.", "OK");
            }
        }
    }
}
