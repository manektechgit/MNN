using Acr.UserDialogs;
using MTNews.Interfaces;
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
    public partial class NewsPage : ContentPage
    {
        public NewsPage()
        {
            try
            {
                InitializeComponent();
                this.BindingContext = new NewsViewModel(this);
                NavigationPage.SetHasNavigationBar(this, false);
            }
            catch (Exception ex)
            {
                Logger.SendErrorLog(ex);
            }
        }

        protected async override void OnAppearing()
        {
            try
            {
                base.OnAppearing();
                UserDialogs.Instance.ShowLoading();
                await (this.BindingContext as NewsViewModel).GetAllNews();
                UserDialogs.Instance.HideLoading();
            }
            catch (Exception ex)
            {
                Logger.SendErrorLog(ex);
            }
        }
        protected override void OnDisappearing()
        {
            base.OnDisappearing();
            try
            {
                (this.BindingContext as NewsViewModel).IsSearchedText = false;
                (this.BindingContext as NewsViewModel).IsSearchedTapped = false;
                (this.BindingContext as NewsViewModel).SearchText = string.Empty;
            }
            catch (Exception ex)
            {
                Logger.SendErrorLog(ex);
            }
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