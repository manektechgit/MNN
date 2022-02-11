using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MTNews.CustomViews
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class HeaderView : ContentView
    {
        public HeaderView()
        {
            InitializeComponent();
        }

        private void CustomEntry_Completed(object sender, EventArgs e)
        {
            try
            {
                if (!string.IsNullOrEmpty(searchEntry.Text))
                {
                    (this.BindingContext as NewsViewModel).OnSearchTapClicked(null);
                }
            }
            catch (Exception ex)
            {
                Logger.SendErrorLog(ex);
            }
        }
    }
}