using Microsoft.Maui.Controls;

namespace Project2._3
{
    public partial class EditItemPage : ContentPage
    {
        private Item itemToEdit;

        public EditItemPage(Item item)
        {
            InitializeComponent();
            itemToEdit = item;
            LoadItemData();
        }

        private void LoadItemData()
        {
            ItemIdLabel.Text = itemToEdit.GetId().ToString();
            NameEntry.Text = itemToEdit.GetName();
            DescriptionEntry.Text = itemToEdit.GetDescription();
            PriceEntry.Text = itemToEdit.GetPrice().ToString();
            AmountEntry.Text = itemToEdit.GetAmountAvailable().ToString();
        }

        private async void OnSaveClicked(object sender, EventArgs e)
        {
            try
            {
                // Validate inputs
                if (string.IsNullOrWhiteSpace(NameEntry.Text))
                {
                    await DisplayAlert("Validation Error", "Please enter a name for the item.", "OK");
                    return;
                }

                if (string.IsNullOrWhiteSpace(DescriptionEntry.Text))
                {
                    await DisplayAlert("Validation Error", "Please enter a description for the item.", "OK");
                    return;
                }

                if (!double.TryParse(PriceEntry.Text, out double price) || price < 0)
                {
                    await DisplayAlert("Validation Error", "Please enter a valid price (0 or greater).", "OK");
                    return;
                }

                if (!int.TryParse(AmountEntry.Text, out int amount) || amount < 0)
                {
                    await DisplayAlert("Validation Error", "Please enter a valid quantity (0 or greater).", "OK");
                    return;
                }

                // Show confirmation
                string confirmationMessage = $"Save changes to:\n\n" +
                    $"Name: {NameEntry.Text}\n" +
                    $"Description: {DescriptionEntry.Text}\n" +
                    $"Price: {price:C}\n" +
                    $"Quantity: {amount}";

                bool confirm = await DisplayAlert("Confirm Changes", confirmationMessage, "Save", "Cancel");

                if (confirm)
                {
                    // Update the item
                    itemToEdit.SetName(NameEntry.Text);
                    itemToEdit.SetDescription(DescriptionEntry.Text);
                    itemToEdit.SetPrice(price);
                    itemToEdit.SetAmountAvailable(amount);

                    await DisplayAlert("Success", "Item has been updated successfully!", "OK");
                    await Navigation.PopAsync(); // Go back to previous page
                }
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", $"An error occurred while saving: {ex.Message}", "OK");
            }
        }

        private async void OnCancelClicked(object sender, EventArgs e)
        {
            bool confirm = await DisplayAlert("Cancel Edit", 
                "Are you sure you want to cancel? All changes will be lost.", "Yes", "No");
            
            if (confirm)
            {
                await Navigation.PopAsync(); // Go back without saving
            }
        }
    }
}