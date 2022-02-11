using Rg.Plugins.Popup.Pages;
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
    public partial class UsersPopup : PopupPage
    {
        public UsersPopup()
        {
            try
            {
                InitializeComponent();
                }
            catch (Exception ex)
            {
                Logger.SendErrorLog(ex);
            }
        }
    }
}