using MTNews.Services;
using MTNews.Views;
using System;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MTNews
{
    public partial class App : Application
    {
        public static NavigationPage NavigationPage { get; private set; }
        public static MenuPage RootPage;
        public static MenuPageFlyout MenuPageFlyout;
        public static bool MenuIsPresented
        {
            get
            {
                return RootPage.IsPresented;
            }
            set
            {
                RootPage.IsPresented = value;
            }
        }

        public App()
        {
            try
            {
                InitializeComponent();

                RegisterDependencies();

                var IsLoggedIn = Preferences.Get(Constants.IsLoggedIn, false);

                if (IsLoggedIn)
                    CallMainPage();
                else
                    MainPage = new NavigationPage(new LoginPage());
            }
            catch (Exception ex)
            {
                Logger.SendErrorLog(ex);
            }
        }
        public static void CallMainPage()
        {
            try
            {
                MenuPageFlyout = new MenuPageFlyout();
                NavigationPage = new NavigationPage(new NewsPage());
                RootPage = new MenuPage();
                RootPage.Flyout = MenuPageFlyout;
                RootPage.Detail = NavigationPage;
                Current.MainPage = RootPage;
            }
            catch (Exception ex)
            {
                Logger.SendErrorLog(ex);
            }
        }

        public static void CallDetails(Page pg)
        {
            try
            {
                NavigationPage = new NavigationPage(pg);
                RootPage.Detail = NavigationPage;
            }
            catch (Exception ex)
            {
                Logger.SendErrorLog(ex);
            }
        }
        public void RegisterDependencies()
        {
            try
            {
                DependencyService.Register<IBackendConnection, BackendConnectionImpl>();
            }
            catch (Exception ex)
            {
                Logger.SendErrorLog(ex);
            }
        }

        protected override void OnStart()
        {
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}
