using System.Collections.ObjectModel;
using RestaurantApp_FullImp.Project.Models;
using MenuItem = RestaurantApp_FullImp.Project.Models.MenuItem;

namespace RestaurantApp_FullImp.Project.Controllers
{
    public class MenuController
    {
        private List<MenuItem> _menuItems;
        private DatabaseHelper _database;

        public MenuController()
        {
            string dbPath = Path.Combine(FileSystem.AppDataDirectory, "restaurant.db3");
            _database = new DatabaseHelper(dbPath);

            _menuItems = LoadMenuItems();
        }

        public List<MenuItem> GetItems(MenuItemType? type = null)
        {
            if (type == null)
                return new List<MenuItem>(_menuItems);
            else
            {
                var result = from item in _menuItems
                             where item.Type == type
                             select item;

                return result.ToList();
            }
        }

        public void AddMenuItem(MenuItem item)
        {
            var dbItem = ToDBModel(item);
            _database.AddMenuItem(dbItem);
            _menuItems.Add(item);
        }

        public void UpdateMenuItem(MenuItem item)
        {
            var dbItem = ToDBModel(item);
            _database.UpdateMenuItem(dbItem);
            var oldItem = _menuItems.FirstOrDefault(x => x.DatabaseID == item.DatabaseID);
            if (oldItem != null)
            {
                _menuItems.Remove(oldItem);
                _menuItems.Add(item);
            }
        }

        public void DeleteMenuItem(MenuItem item)
        {
            var dbItem = ToDBModel(item);
            _database.DeleteMenuItem(dbItem);
            _menuItems.RemoveAll(x => x.DatabaseID == item.DatabaseID);
        }

        private List<MenuItem> LoadMenuItems()
        {
            var dbItems = _database.GetMenuItems();

            if (dbItems.Count == 0)
            {
                // No data in database? Populate starter menu
                var starterItems = setup_menu();
                foreach (var item in starterItems)
                {
                    AddMenuItem(item);
                }
                return starterItems;
            }
            else
            {
                return dbItems.Select(x => ToAppModel(x)).ToList();
            }
        }

        // Convert MenuItemDB (database) -> MenuItem (app)
        private MenuItem ToAppModel(MenuItemDB dbItem)
        {
            return new MenuItem
            {
                DatabaseID = dbItem.DatabaseID,
                ItemName = dbItem.ItemName,
                ItemPrice = dbItem.ItemPrice,
                HasSize = dbItem.HasSize,
                Size = (MenuSizeType)dbItem.Size,
                Type = (MenuItemType)dbItem.Type,
                Icon = dbItem.Icon
            };
        }

        // Convert MenuItem (app) -> MenuItemDB (database)
        private MenuItemDB ToDBModel(MenuItem item)
        {
            return new MenuItemDB
            {
                DatabaseID = item.DatabaseID,
                ItemName = item.ItemName,
                ItemPrice = item.ItemPrice,
                HasSize = item.HasSize,
                Size = (int)item.Size,
                Type = (int)item.Type,
                Icon = item.Icon
            };
        }

        List<MenuItem> setup_menu()
        {
            return new List<MenuItem>()
            {
                new MenuItem { ItemName = "Seafood Alfredo", Type = MenuItemType.ENTREE, ItemPrice=15.95M},
                new MenuItem { ItemName = "Chicken Alfredo", Type = MenuItemType.ENTREE, ItemPrice=13.95M },
                new MenuItem { ItemName = "Chicken Picatta", Type = MenuItemType.ENTREE, ItemPrice=13.95M },
                new MenuItem { ItemName = "Turkey Club", Type = MenuItemType.ENTREE, ItemPrice=11.95M },
                new MenuItem { ItemName = "Lobster Pie", Type = MenuItemType.ENTREE, ItemPrice=19.95M },
                new MenuItem { ItemName = "Prime Rib", Type = MenuItemType.ENTREE, ItemPrice=20.95M },
                new MenuItem { ItemName = "Shrimp Scampi", Type = MenuItemType.ENTREE, ItemPrice=18.95M },
                new MenuItem { ItemName = "Turkey Dinner", Type = MenuItemType.ENTREE, ItemPrice=13.95M },
                new MenuItem { ItemName = "Stuffed Chicken", Type = MenuItemType.ENTREE, ItemPrice=14.95M },
                new MenuItem { ItemName = "Classic Fries", Type = MenuItemType.SIDE, ItemPrice=1.95M, HasSize=true },
                new MenuItem { ItemName = "Spicy Fries", Type = MenuItemType.SIDE, ItemPrice=2.50M, HasSize=true },
                new MenuItem { ItemName = "Mashed Potatoes", Type = MenuItemType.SIDE, ItemPrice=1.95M, HasSize=true },
                new MenuItem { ItemName = "Steamed Vegetables", Type = MenuItemType.SIDE, ItemPrice=2.50M, HasSize=true },
                new MenuItem { ItemName = "Garden Salad", Type = MenuItemType.SIDE, ItemPrice=2.95M },
                new MenuItem { ItemName = "Loaded Potatos", Type = MenuItemType.SIDE, ItemPrice=3.50M },
                new MenuItem { ItemName = "Mac and Cheese", Type = MenuItemType.SIDE , ItemPrice=2.50M, HasSize=true },
                new MenuItem { ItemName = "Soda", Type = MenuItemType.DRINK, ItemPrice=1.95M, HasSize=true },
                new MenuItem { ItemName = "Tea", Type = MenuItemType.DRINK, ItemPrice=1.50M, HasSize=true },
                new MenuItem { ItemName = "Coffee", Type = MenuItemType.DRINK, ItemPrice=1.25M, HasSize=true },
                new MenuItem { ItemName = "Mineral Water", Type = MenuItemType.DRINK, ItemPrice=2.95M, HasSize=true },
                new MenuItem { ItemName = "Juice", Type = MenuItemType.DRINK, ItemPrice=2.50M, HasSize=true },
                new MenuItem { ItemName = "Buffalo Wings", Type = MenuItemType.APPETIZER, ItemPrice=15.95M },
                new MenuItem { ItemName = "Buffalo Fingers", Type = MenuItemType.APPETIZER, ItemPrice=15.95M },
                new MenuItem { ItemName = "Potato Skins", Type = MenuItemType.APPETIZER, ItemPrice=15.95M },
                new MenuItem { ItemName = "Nachos", Type = MenuItemType.APPETIZER, ItemPrice=15.95M },
                new MenuItem { ItemName = "Mushrooms Caps", Type = MenuItemType.APPETIZER, ItemPrice=15.95M },
                new MenuItem { ItemName = "Shrimp Cocktail", Type = MenuItemType.APPETIZER, ItemPrice=15.95M },
                new MenuItem { ItemName = "Chips and Salsa", Type = MenuItemType.APPETIZER, ItemPrice=15.95M },
                new MenuItem { ItemName = "Apple Pie", Type = MenuItemType.DESSERT, ItemPrice=5.95M },
                new MenuItem { ItemName = "Sundae", Type = MenuItemType.DESSERT, ItemPrice=3.95M },
                new MenuItem { ItemName = "Carrot Cake", Type = MenuItemType.DESSERT, ItemPrice=5.95M },
                new MenuItem { ItemName = "Mud Pie", Type = MenuItemType.DESSERT , ItemPrice=4.95M },
                new MenuItem { ItemName = "Apple Crisp", Type = MenuItemType.DESSERT, ItemPrice=5.95M }
            };
        }
    }
}
