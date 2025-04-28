/*
I acknowledge the following statements:
That the code I provide below is my own work and NOT copied from any outside resource, which includes, but not limited to, an artificial intelligence program unless given explicit permission by the instructor.
That the code I provide below is my own work and NOT the work of my peers, tutors, or any other individual unless given explicit permission by the instructor.
That if the code below is in violation of statements 1 and 2 above, I may be reported to the Academic Integrity office and subject to penalties as described in the Academic Integrity Policy.
Your Name: Sandesh Bhattarai
Your Student ID: w10166206
*/


using System.Collections.ObjectModel;
using RestaurantApp_FullImp.Project.Views;
using RestaurantApp_FullImp.Project.Models;
using AppMenuItem = RestaurantApp_FullImp.Project.Models.MenuItem; 

/*
    Hint: A class called ItemSelectView is defined that "wraps" around the data model and provides properties 
    that are referenced in the collection view's data template.  The collection view should be an observable
    list of ItemSelectView objects that bind to the menu items being shown.
*/


namespace RestaurantApp_FullImp.Project.Views;

public class SizeOption
{
    public string Text { get; set; } = "";
    public MenuSizeType SizeType { get; set; }
    public string Rate { get; set; } = "";
}

public partial class ItemAddPage : ContentPage
{
      //Add any page attributes here...
    private readonly MenuItemType _categoryToDisplay; 
    private ObservableCollection<ItemSelectView> itemViews;
    private ObservableCollection<SizeOption> sizeOptions;
    private AppMenuItem? selectedMenuItem = null;
    private SizeOption? selectedSize;

    public ItemAddPage(MenuItemType type)
    {
        InitializeComponent();

        //add constructor logic here.

        _categoryToDisplay = type;
        Title = $"Select a {_categoryToDisplay}";

        // Filter items from MenuController
        var filtered = App.Menu.GetItems(type)
            .Select(item => new ItemSelectView { Item = item });

        itemViews = new ObservableCollection<ItemSelectView>(filtered);
        PopulateMenuItems();

        collItemSelection.ItemsSource = itemViews;

        // Define available sizes
        sizeOptions = new ObservableCollection<SizeOption>
        {
            new SizeOption { Text = "Medium", SizeType = MenuSizeType.MEDIUM, Rate = "+25%" },
            new SizeOption { Text = "Large", SizeType = MenuSizeType.LARGE, Rate = "+75%" }
        };
        collSizeSelection.ItemsSource = sizeOptions;

        layoutSizeSelectView.IsVisible = false;
    }
    // Retrieves menu items and adds them to the display collection
    private void PopulateMenuItems()
    {
        var items = App.Menu.GetItems(_categoryToDisplay);

        foreach (var original in items)
        {
            var duplicate = original.DeepCopy();
            itemViews.Add(new ItemSelectView { Item = duplicate });
        }
    }

    private void SelectItem(object sender, SelectionChangedEventArgs e)
    {
        var selectedView = e.CurrentSelection.FirstOrDefault() as ItemSelectView;

        if (selectedView != null)
        {
            selectedMenuItem = selectedView.Item.DeepCopy();

            if (selectedMenuItem.HasSize)
            {
                layoutSizeSelectView.IsVisible = true;
                selectedSize = sizeOptions[0];
                collSizeSelection.SelectedItem = selectedSize;
            }
            else
            {
                layoutSizeSelectView.IsVisible = false;
                selectedSize = null;
            }
        }
        else
        {
            selectedMenuItem = null;
            layoutSizeSelectView.IsVisible = false;
            collSizeSelection.SelectedItem = null;
        }
    }

    private void SelectSize(object sender, SelectionChangedEventArgs e)
    {
        selectedSize = e.CurrentSelection.FirstOrDefault() as SizeOption;

        if (selectedMenuItem != null && selectedSize != null)
        {
            selectedMenuItem.Size = selectedSize.SizeType;
        }
    }

    private async void AddItemClicked(object sender, EventArgs e)
    {
        if (selectedMenuItem == null)
        {
            await DisplayAlert("Error", "Please select an item.", "OK");
            return;
        }

        if (selectedMenuItem.HasSize && selectedSize == null)
        {
            await DisplayAlert("Error", "Please select a size.", "OK");
            return;
        }

        App.Cart.AddItem(selectedMenuItem);
        await DisplayAlert("Success", $"{selectedMenuItem.ItemName} added to cart.", "OK");

        // Navigate back to menu (cart visible)
        App.Current.Windows[0].Page = new MainMenuPage();
    }
}

