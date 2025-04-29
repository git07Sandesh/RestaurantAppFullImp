/*
I acknowledge the following statements:
That the code I provide below is my own work and NOT copied from any outside resource, which includes, but not limited to, an artificial intelligence program unless given explicit permission by the instructor.
That the code I provide below is my own work and NOT the work of my peers, tutors, or any other individual unless given explicit permission by the instructor.
That if the code below is in violation of statements 1 and 2 above, I may be reported to the Academic Integrity office and subject to penalties as described in the Academic Integrity Policy.
Your Name: Sandesh Bhattarai
Your Student ID: w10166206
*/
using RestaurantApp_FullImp.Project.Models;
using RestaurantApp_FullImp.Project.Controllers;
using MenuItem = RestaurantApp_FullImp.Project.Models.MenuItem;

namespace RestaurantApp_FullImp.Project.Views;

public partial class MenuDashboardPage : ContentPage
{
    private List<MenuItem> allItems;
    private MenuController _menuController;

    public MenuDashboardPage()
    {
        InitializeComponent();
        _menuController = App.Menu;
        LoadItems();
    }

    private void LoadItems()
    {
        allItems = _menuController.GetItems();
        menuItemsList.ItemsSource = allItems;
    }

    private void OnSearchTextChanged(object sender, TextChangedEventArgs e)
    {
        ApplyFilters();
    }

    private void OnTypeFilterChanged(object sender, EventArgs e)
    {
        ApplyFilters();
    }

    private void OnSizeSwitchToggled(object sender, ToggledEventArgs e)
    {
        ApplyFilters();
    }

    private void ApplyFilters()
    {
        var filtered = allItems.AsEnumerable();

        // Filter by search
        if (!string.IsNullOrWhiteSpace(searchBar.Text))
            filtered = filtered.Where(i => i.ItemName.Contains(searchBar.Text, StringComparison.OrdinalIgnoreCase));

        // Filter by type
        if (typeFilter.SelectedIndex > 0) // 0 is "All", so skip filtering
        {
            string selectedTypeString = (string)typeFilter.SelectedItem;
            if (Enum.TryParse(selectedTypeString.ToUpper(), out MenuItemType selectedType))
            {
                filtered = filtered.Where(i => i.Type == selectedType);
            }
        }

        // Filter by HasSize
        if (sizeSwitch.IsToggled)
            filtered = filtered.Where(i => i.HasSize);

        menuItemsList.ItemsSource = filtered.ToList();
    }


  private async void OnAddItemClicked(object sender, EventArgs e)
{
    // Ask for item name
    string itemName = await DisplayPromptAsync("New Menu Item", "Enter the name of the item:");
    if (string.IsNullOrWhiteSpace(itemName))
    {
        await DisplayAlert("Error", "Item name cannot be empty.", "OK");
        return;
    }

    // Ask for item price
    string priceInput = await DisplayPromptAsync("New Menu Item", "Enter the price of the item:");
    if (!decimal.TryParse(priceInput, out decimal itemPrice))
    {
        await DisplayAlert("Error", "Invalid price entered.", "OK");
        return;
    }

    // Create new item
    var newItem = new MenuItem
    {
        ItemName = itemName,
        ItemPrice = itemPrice,
        HasSize = false,  // You can ask for more details if needed
        Size = MenuSizeType.SMALL,  // Default size
        Type = MenuItemType.ENTREE, // Default type
        Icon = "eagle.png"           // Default icon
    };

    _menuController.AddMenuItem(newItem);
    LoadItems();
    await DisplayAlert("Added", "New menu item added.", "OK");
}
 private async void OnEditItemClicked(object sender, EventArgs e)
{
    var selectedItem = menuItemsList.SelectedItem as MenuItem;
    if (selectedItem == null)
    {
        await DisplayAlert("Error", "Select an item first.", "OK");
        return;
    }

    // Ask for new name
    string newName = await DisplayPromptAsync("Edit Item", "Enter new name:", initialValue: selectedItem.ItemName);
    if (string.IsNullOrWhiteSpace(newName))
    {
        await DisplayAlert("Error", "Name cannot be empty.", "OK");
        return;
    }

    // Ask for new price
    string priceInput = await DisplayPromptAsync("Edit Item", "Enter new price:", initialValue: selectedItem.ItemPrice.ToString());
    if (!decimal.TryParse(priceInput, out decimal newPrice))
    {
        await DisplayAlert("Error", "Invalid price.", "OK");
        return;
    }

    // Ask for new type (optional, just keeping same for now)
    // Optionally you can add another prompt for type and size if needed

    // Update the selected item
    selectedItem.ItemName = newName;
    selectedItem.ItemPrice = newPrice;

    _menuController.UpdateMenuItem(selectedItem);
    LoadItems();
    await DisplayAlert("Edited", "Item updated successfully.", "OK");
}


    private async void OnDeleteItemClicked(object sender, EventArgs e)
    {
        var selectedItem = menuItemsList.SelectedItem as MenuItem;
        if (selectedItem != null)
        {
            _menuController.DeleteMenuItem(selectedItem);
            LoadItems();
            await DisplayAlert("Deleted", "Item removed.", "OK");
        }
        else
        {
            await DisplayAlert("Error", "Select an item first.", "OK");
        }
    }
}
