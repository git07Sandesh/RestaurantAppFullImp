using SQLite;
using RestaurantApp_FullImp.Project.Models;

namespace RestaurantApp_FullImp.Project.Controllers
{
    public class DatabaseHelper
    {
        private readonly SQLiteConnection _database;

        public DatabaseHelper(string dbPath)
        {
            _database = new SQLiteConnection(dbPath);
            _database.CreateTable<MenuItemDB>();
        }

        public List<MenuItemDB> GetMenuItems()
        {
            return _database.Table<MenuItemDB>().ToList();
        }

        public int AddMenuItem(MenuItemDB item)
        {
            return _database.Insert(item);
        }

        public int UpdateMenuItem(MenuItemDB item)
        {
            return _database.Update(item);
        }

        public int DeleteMenuItem(MenuItemDB item)
        {
            return _database.Delete(item);
        }
    }

    public class MenuItemDB
    {
        [PrimaryKey, AutoIncrement]
        public int DatabaseID { get; set; }
        public string ItemName { get; set; }
        public decimal ItemPrice { get; set; }
        public bool HasSize { get; set; }
        public int Size { get; set; } // 0 - Small, 1 - Medium, 2 - Large
        public int Type { get; set; } // 0 - Entrée, 1 - Side, etc.
        public string Icon { get; set; }
    }
}
