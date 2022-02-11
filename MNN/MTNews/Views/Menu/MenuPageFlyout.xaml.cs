using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MTNews.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MenuPageFlyout : ContentPage
    {
        public CollectionView ListView;

        public MenuPageFlyout()
        {
            try
            {
                InitializeComponent();

                BindingContext = new MenuPageFlyoutViewModel();
                ListView = MenuItemsListView;
                NavigationPage.SetHasNavigationBar(this, false);
            }
            catch (Exception ex)
            {
                Logger.SendErrorLog(ex);
            }
        }

        public class MenuPageFlyoutViewModel : BaseViewModel
        {
            private ObservableCollection<MenuPageFlyoutMenuItem> menuItems;
            public ObservableCollection<MenuPageFlyoutMenuItem> MenuItems
            {
                get { return menuItems; }
                set { SetProperty(ref menuItems, value); }
            }
            public MenuPageFlyoutViewModel()
            {
                try
                {
                    MenuItems = new ObservableCollection<MenuPageFlyoutMenuItem>(new[]
                    {
                        new MenuPageFlyoutMenuItem { Id = 1, Title = "Home", TargetType = typeof(NewsPage), IconSource="news" },
                        new MenuPageFlyoutMenuItem { Id = 2, Title = "Profile", TargetType = typeof(ProfilePage), IconSource="profile" }
                        //new MenuPageFlyoutMenuItem { Id = 3, Title = "Reset Password", TargetType = typeof(ResetPasswordPage), IconSource="reset" },
                        //new MenuPageFlyoutMenuItem { Id = 4, Title = "Terms & Conditions", TargetType = typeof(TermsConditionPage), IconSource="terms" },
                        //new MenuPageFlyoutMenuItem { Id = 5, Title = "Support", TargetType = typeof(SupportPage), IconSource="support" }
                    });
                }
                catch (Exception ex)
                {
                    Logger.SendErrorLog(ex);
                }
            }
        }
    }
}