using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MTNews.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MenuPage : FlyoutPage
    {
        public MenuPage()
        {
            try
            {
                InitializeComponent();
                App.MenuPageFlyout.ListView.SelectionChanged += ListView_SelectionChanged; 
                NavigationPage.SetHasNavigationBar(this, false);
            }
            catch (Exception ex)
            {
                Logger.SendErrorLog(ex);
            }
        }

        private void ListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {           
            try
            {
                var item = e.CurrentSelection.LastOrDefault() as MenuPageFlyoutMenuItem;
                if (item == null)
                    return;

                var page = (Page)Activator.CreateInstance(item.TargetType);
                page.Title = item.Title;

                App.CallDetails(page);

                IsPresented = false;

                App.MenuPageFlyout.ListView.SelectedItem = null;
            }
            catch (Exception ex)
            {
                Logger.SendErrorLog(ex);
            }
        }
    }
}