﻿using RestaurantApp_FullImp.Project.Models;
using MenuItem = RestaurantApp_FullImp.Project.Models.MenuItem;

namespace RestaurantApp_FullImp.Project.Views
{

    public class ComboItemView : BindableObject
    {
        public MenuItem Item { get; set; }

        public string ItemName
        {
            get
            {
                return $"{Item.ItemName}\n${Item.GetCost():F2}";
            }
        }

        public string Icon
        {
            get { return Item.Icon; }
        }
    }

    class ItemSelectView : BindableObject
    {
        public MenuItem Item { get; set; }

        public string ItemName
        {
            get
            {
                return $"{Item.ItemName}\n${Item.GetCost():F2}";
            }
        }

        public string Icon
        {
            get { return Item.Icon; }
        }
    }

    public class SizeTypeView : BindableObject
    {
        public string Text { get; set; } = "";
        public string Rate { get; set; } = "";
    }

    public partial class CartPopupItemView : BindableObject
    {
        public CartItem Item { get; set; }
        public string Detail
        {

            get
            {
                if ((Item as RestaurantApp_FullImp.Project.Models.MenuItem) != null)
                {
                    //output just name of item.
                    return (Item as RestaurantApp_FullImp.Project.Models.MenuItem).ItemName;

                }
                else if ((Item as RestaurantApp_FullImp.Project.Models.ComboItem) != null)
                {
                    //output combo details
                    string ss = "";
                    ss += "** Combo **\n";
                    ss += " --> ";
                    ss += (Item as RestaurantApp_FullImp.Project.Models.ComboItem).Entree.ItemName;
                    ss += "\n";
                    ss += " --> ";
                    ss += (Item as RestaurantApp_FullImp.Project.Models.ComboItem).Side.ItemName;
                    ss += "\n";
                    ss += " --> ";
                    ss += (Item as RestaurantApp_FullImp.Project.Models.ComboItem).Drink.ItemName;
                    ss += "\n";

                    return ss;
                }
                else
                    return "";
            }
        }

        public string ItemCost
        {
            get
            {
                return $"${Item.GetCost():F2}";
            }
        }
    }
}
