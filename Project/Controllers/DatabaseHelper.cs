
// I acknowledge the following statements:

// 1. That the code I provide below is my own work and NOT copied from any outside resource, which includes, but not limited to, an artificial intelligence program unless given explicit permission by the instructor.

// 2. That the code I provide below is my own work and NOT the work of my peers, tutors, or any other individual unless given explicit permission by the instructor.

// 3. That if the code below is in violation of statements 1 and 2 above, I may be reported to the Academic Integrity office and subject to penalties as described in the Academic Integrity Policy.

// Your Name: Sandesh Bhattarai

// Your Student ID: w10166206


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
