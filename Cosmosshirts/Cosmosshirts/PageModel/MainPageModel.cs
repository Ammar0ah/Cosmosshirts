using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Text;
using System.Threading.Tasks;

using Cosmosshirts.Page;
using Cosmosshirts.Services;
using FreshMvvm;
using ReactiveUI;
using Xamarin.Forms;
using MenuItem = Cosmosshirts.Models.MenuItem;
using PropertyChangingEventArgs = ReactiveUI.PropertyChangingEventArgs;
using PropertyChangingEventHandler = ReactiveUI.PropertyChangingEventHandler;

namespace Cosmosshirts.PageModel
{
    public class MainPageModel : BasePageModel
    {
        private IDatabaseService _databaseService;
        private MenuItem _menuItem;
        public ObservableCollection<MenuItem> MenuItems { get; set; }
        public ReactiveCommand SelecteditemCommand;

        public MenuItem MenuItem
        {
            get => _menuItem;
            set => this.RaiseAndSetIfChanged(ref _menuItem, value);

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
        public MainPageModel(IDatabaseService databaseService)
        {  
           this. _databaseService = databaseService;

            MenuItems = new ObservableCollection<MenuItem>(GetMenuItems());
            SelecteditemCommand = ReactiveCommand.CreateFromTask<SelectedItemChangedEventArgs>(ChooseCommand);

        }
        
        async Task ChooseCommand(SelectedItemChangedEventArgs args)
        {
            var item = args.SelectedItem as MenuItem;
            var page = CurrentPage as MainPage;
            if (item.Label == "جديدنا")
                await CoreMethods.PushPageModel<NewProductsPageModel>();
            if (item.Label == "جميع المنتجات")
                await CoreMethods.PushPageModel<AllProductsPageModel>();
            if (item.Label == "اتصل بنا")
                await CoreMethods.PushPageModel<ContactUsPageModel>();
           
       
             page.DeselectItem();

        }
    }
}
