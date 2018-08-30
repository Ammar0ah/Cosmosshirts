using System;
using System.Collections.ObjectModel;
using CommonServiceLocator;
using Cosmosshirts.Helpers;
using Cosmosshirts.PageModel;
using Cosmosshirts.Page;
using Cosmosshirts.Services;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using FreshMvvm;
using ReactiveUI;
using Unity;
using Unity.ServiceLocation;

[assembly: XamlCompilation(XamlCompilationOptions.Compile)]
namespace Cosmosshirts
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();
           // myImageSource = "Cosmosshirts.Assits.menu.png";
          
            FreshPageModelResolver.PageModelMapper = new FreshPageModelMapper();

        }

        void SetupPage()
        {
            //FreshIOC.Container.Register<IDatabaseService, DatabaseService>();
            //var mainmenuPage = FreshPageModelResolver.ResolvePageModel<MainPageModel>();

            //mainmenuPage.Title = "القائمة";
            //var MasterPageArea = new FreshNavigationContainer(mainmenuPage, "MainMenuContainer");
            //MasterPageArea.Title = "القائمة";
            //MasterPageArea.Icon = "menu.png";
            //var content = FreshPageModelResolver.ResolvePageModel<AllProductsPageModel>();
            //var masterdetailpage = new FreshMasterDetailNavigationContainer();

            //masterdetailpage.Master = MasterPageArea;

            //MainPage = masterdetailpage;

            //UnityContainer unityContainer = new UnityContainer();
            //unityContainer.RegisterType<IDatabaseService, DatabaseService>();
            //ServiceLocator.SetLocatorProvider(() => new UnityServiceLocator(unityContainer));
            //MainPage = new MainPage(_databaseService);
           var masterDetailNav = new MasterDetailNavigationContainer();
            masterDetailNav.Init("Menu");
            masterDetailNav.AddPage<AllProductsPageModel>("All products","menu.png");
            masterDetailNav.AddPage<NewProductsPageModel>("New products","menu.png");
            masterDetailNav.AddPage<ContactUsPageModel>("ContactUs","menu.png");
        }
        protected override void OnStart()
        {
            SetupPage();
            // Handle when your app starts
        }

        protected override void OnSleep()
        {
            // Handle when your app sleeps
        }

        protected override void OnResume()
        {
            // Handle when your app resumes
        }
    }
}
