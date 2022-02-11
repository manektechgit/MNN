using MTNews.Models;
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
    public partial class NewsDetailsPage : ContentPage
    {
        public NewsDetailsPage(NewsResponseData news)
        {
            try
            {
                InitializeComponent();
                this.BindingContext = new NewsDetailsViewModel(news, this);
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

                await (this.BindingContext as NewsDetailsViewModel).UpdateNewsViews();
                await (this.BindingContext as NewsDetailsViewModel).GetAllComments();
                await (this.BindingContext as NewsDetailsViewModel).GetLikeStatus();
                await (this.BindingContext as NewsDetailsViewModel).GetNewsById();
            }
            catch (Exception ex)
            {
                Logger.SendErrorLog(ex);
            }
        }
        private async void DeleteTapped(object sender, EventArgs e)
        {
            try
            {
                var data = ((e as TappedEventArgs).Parameter) as CommentsResponseData;

                if (data != null)
                {
                    await (this.BindingContext as NewsDetailsViewModel).DeleteComments(data);
                }
            }
            catch (Exception ex)
            {
                Logger.SendErrorLog(ex);
            }
        }
        private void EditTapped(object sender, EventArgs e)
        {
            try
            {
                var data = ((e as TappedEventArgs).Parameter) as CommentsResponseData;

                if (data != null)
                {
                    (this.BindingContext as NewsDetailsViewModel).UpdatedComment = data.comment;
                    (this.BindingContext as NewsDetailsViewModel).CommentID = data.id;
                    (this.BindingContext as NewsDetailsViewModel).OpenPopup();
                }
            }
            catch (Exception ex)
            {
                Logger.SendErrorLog(ex);
            }
        }

        public async Task ScrollToComments()
        {
            try
            {
                await scrollViews.ScrollToAsync(0, boxView.Y, true);
            }
            catch (Exception ex)
            {
                Logger.SendErrorLog(ex);
            }
        }
    }
}