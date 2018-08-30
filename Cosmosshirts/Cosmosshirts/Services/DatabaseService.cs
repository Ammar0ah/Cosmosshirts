using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;
using MenuItem = Cosmosshirts.Models.MenuItem;

namespace Cosmosshirts.Services
{
    public class DatabaseService :IDatabaseService
    {
        private List<MenuItem> menuItems;
        public DatabaseService()
        {
            menuItems = GetMenuItems();
        }
        public List<MenuItem> GetMenuItems()
        {
            return new List<MenuItem>
            {
                new MenuItem(){Label = "جديدنا" , MenuItemImage = ImageSource.FromResource("Cosmosshirts.Assets.image-placeholder.png")},
                new MenuItem(){Label = "جميع المنتجات" , MenuItemImage =ImageSource.FromResource("Cosmosshirts.Assets.image-placeholder.png")},
                new MenuItem(){Label = "اتصل بنا" , MenuItemImage =ImageSource.FromResource("Cosmosshirts.Assets.image-placeholder.png")},
            };
        }
    }
}
