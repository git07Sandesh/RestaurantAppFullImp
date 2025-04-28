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
        if (typeFilter.SelectedIndex > 0)
        {
            var selectedType = (MenuItemType)(typeFilter.SelectedIndex - 1);
            filtered = filtered.Where(i => i.Type == selectedType);
        }

        // Filter by HasSize
        if (sizeSwitch.IsToggled)
            filtered = filtered.Where(i => i.HasSize);

        menuItemsList.ItemsSource = filtered.ToList();
    }

    private async void OnAddItemClicked(object sender, EventArgs e)
    {
        var newItem = new MenuItem
        {
            ItemName = "New Item",
            ItemPrice = 0.0M,
            HasSize = false,
            Size = MenuSizeType.SMALL,
            Type = MenuItemType.ENTREE,
            Icon = "eagle.png"
        };

        _menuController.AddMenuItem(newItem);
        LoadItems();
        await DisplayAlert("Added", "New menu item added.", "OK");
    }

    private async void OnEditItemClicked(object sender, EventArgs e)
    {
        var selectedItem = menuItemsList.SelectedItem as MenuItem;
        if (selectedItem != null)
        {
            // Simple example: increase price
            selectedItem.ItemPrice += 1.00M;
            _menuController.UpdateMenuItem(selectedItem);
            LoadItems();
            await DisplayAlert("Edited", "Item updated (Price +1)", "OK");
        }
        else
        {
            await DisplayAlert("Error", "Select an item first.", "OK");
        }
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
