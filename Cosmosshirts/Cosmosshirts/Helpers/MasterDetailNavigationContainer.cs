using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Cosmosshirts.Page;
using Cosmosshirts.Services;
using FreshMvvm;
using Xamarin.Forms;
using Page = Xamarin.Forms.Page;
namespace Cosmosshirts.Helpers
{
    public class LazyLoadedPage
    {
        public string Title { get; set; }
        public string Icon { get; set; }

        public Xamarin.Forms.Page Page { get; set; }
        public Type ViewModelType { get; set; }
        public Object Data { get; set; }
        public string Group { get; set; }
    }

    public class Grouping<K, T> : List<T>
    {
        public K Key { get; set; }

        public Grouping(K key, List<T> items)
        {
            Key = key;
            foreach (var item in items)
                this.Add(item);
        }
    }
    class MasterDetailNavigationContainer : MasterDetailPage, IFreshNavigationService
    {
        private MainPage _masterView;
        private ListView _listView;
        public IDatabaseService _databaseService;
        public ObservableCollection<LazyLoadedPage> Pages { get; } = new ObservableCollection<LazyLoadedPage>();
        public string NavigationServiceName { get; }

        public MasterDetailNavigationContainer() : this(Constants.DefaultNavigationServiceName)
        {

        }

        public MasterDetailNavigationContainer(string navigationServiceName)
        {
            NavigationServiceName = navigationServiceName;

        }

        public MasterDetailNavigationContainer(string navigationServiceName,IDatabaseService databaseService)
        {
            NavigationServiceName = navigationServiceName;
            _databaseService = databaseService;
        }

        public void Init(string menuTitle, string menuIcon = null)
        {
            CreateMenuPage(menuTitle, menuIcon);
            CreateDetailPage();
            RegisterNavigation();
        }

        protected virtual void RegisterNavigation()
        {
            FreshIOC.Container.Register<IFreshNavigationService>(this, NavigationServiceName);
        }

        public virtual void AddPage(string modelName, string title, string icon = null, object data = null)
        {
            var pageToAdd = new LazyLoadedPage()
            {
                ViewModelType = Type.GetType(modelName),
                Title = title,
                Icon = icon,
                Data = data
            };
            Pages.Add(pageToAdd);
            if (Pages.Count == 1)
                Detail = ResolvePage(pageToAdd);
        }

        public virtual void AddPage<T>(string modelName, string title, string icon = null, object data = null)
            where T : FreshBasePageModel
        {
            var pageToAdd = new LazyLoadedPage()
            {
                ViewModelType = Type.GetType(modelName),
                Data = data,
                Icon = icon,
                Title = title
            };
            Pages.Add(pageToAdd);
            if (Pages.Count == 1)
                Detail = ResolvePage(pageToAdd);

            _listView.ItemsSource = Pages.GroupBy(item => item.Group)
                .Select(item => new Grouping<string, LazyLoadedPage>(item.Key, item.ToList())).ToList();
        }

        internal Xamarin.Forms.Page CreateContainerPageSafe(Xamarin.Forms.Page page)
        {
            if (page is NavigationPage || page is MasterDetailPage || page is TabbedPage)
                return page;

            return CreateContainerPage(page);
        }

        protected virtual Xamarin.Forms.Page CreateContainerPage(Xamarin.Forms.Page page)
        {
            return new NavigationPage(page);
        }

        protected virtual void CreateMenuPage(string menuPageTitle, string menuIcon = null)
        {
           _masterView = new MainPage(_databaseService);
            _listView = _masterView.FindByName<ListView>("MenuListView");

            var source = Pages.GroupBy(item => item.Group)
                .Select(item => new Grouping<string, LazyLoadedPage>(item.Key, item.ToList())).ToList();

            _listView.ItemTapped += (sender, args) =>
            {
                var lazyLoadedPage = (LazyLoadedPage) args.Item;
                 
                if (Pages.Contains(lazyLoadedPage))
                {
                    var page = lazyLoadedPage.Page;

                    if (page == null)
                        page = ResolvePage(lazyLoadedPage);

                    Detail = page;

                }

                IsPresented = false;
            };
            // menuPage.Content = _listView
            var navPage = new NavigationPage(_masterView){Title = menuPageTitle};
            if (!string.IsNullOrEmpty(menuIcon))
            {
                navPage.Icon = menuIcon;
                Icon = menuIcon;
            }

            Master = navPage;
        }
        protected virtual void CreateDetailPage()
        {
            Detail = CreateContainerPage(new Xamarin.Forms.Page());
        }
        protected virtual Xamarin.Forms.Page ResolvePage(LazyLoadedPage lazyLoadedPage)
        {
            var innerPage = FreshPageModelResolver.ResolvePageModel(lazyLoadedPage.ViewModelType, lazyLoadedPage.Data); /////// null Reference exception here!! 
            innerPage.GetModel().CurrentNavigationServiceName = NavigationServiceName;
            return CreateContainerPage(innerPage);
        }

        public Task PopToRoot(bool animate = true)
        {
            return (Detail as NavigationPage).PopToRootAsync(animate);
        }

        public Task PushPage(Xamarin.Forms.Page page, FreshBasePageModel model, bool modal = false, bool animate = true)
        {
            if (modal)
                return Navigation.PushModalAsync(CreateContainerPageSafe(page));
            return  (Detail as NavigationPage).PushAsync(page, animate); //TODO: make this better
        }

        public Task PopPage(bool modal = false, bool animate = true)
        {

            if (modal)
                return Navigation.PopModalAsync(animate);
            return (Detail as NavigationPage).PopAsync(animate); //TODO: make this better            
        }

        public Task<FreshBasePageModel> SwitchSelectedRootPageModel<T>() where T : FreshBasePageModel
        {
            var lazyLoadedPage = Pages.FirstOrDefault(o => o.Page.GetModel().GetType().FullName == typeof(T).FullName);
            var page = lazyLoadedPage.Page;

            if (page == null)
                page = ResolvePage(lazyLoadedPage);

            _listView.SelectedItem = page;

            return Task.FromResult((Detail as NavigationPage).CurrentPage.GetModel());
        }

        public void NotifyChildrenPageWasPopped()
        {
            if (Master is NavigationPage masterNavPage)
                masterNavPage.NotifyAllChildrenPopped();
            if (Master is IFreshNavigationService masterFreshNavPage)
                masterFreshNavPage.NotifyChildrenPageWasPopped();

            foreach (var page in this.Pages)
            {
                if (page.Page is NavigationPage navPage)
                    navPage.NotifyAllChildrenPopped();
                if (page.Page is IFreshNavigationService freshNavPage)
                    freshNavPage.NotifyChildrenPageWasPopped();
            }
            if (Pages != null && !Pages.Any(item => item.Page == Detail) && Detail is NavigationPage)
                ((NavigationPage)Detail).NotifyAllChildrenPopped();
            if (Pages != null && !Pages.Any(item => item.Page == Detail) && Detail is IFreshNavigationService)
                ((IFreshNavigationService)Detail).NotifyChildrenPageWasPopped();
        }


    }
}
