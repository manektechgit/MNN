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
    public partial class ProfilePage : ContentPage
    {
        public ProfilePage()
        {
            try
            {
                InitializeComponent();
                this.BindingContext = new ProfileViewModel();
                NavigationPage.SetHasNavigationBar(this, false);
            }
            catch (Exception ex)
            {
                Logger.SendErrorLog(ex);
            }
        }
    }
}