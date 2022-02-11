using Acr.UserDialogs;
using MTNews.Interfaces;
using MTNews.Models;
using MTNews.Services;
using MTNews.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace MTNews
{
    public class NewsViewModel : BaseViewModel
    {
        #region Properties
        IBackendConnection newsService = DependencyService.Get<IBackendConnection>();
        public int PageNumber = 0;
        public int PageSize = 10;
        public NewsPage pg;

        private ObservableCollection<NewsResponseData> newsFeeds;
        public ObservableCollection<NewsResponseData> NewsFeeds
        {
            get { return newsFeeds; }
            set { SetProperty(ref newsFeeds, value); }
        }
        private string searchText;
        public string SearchText
        {
            get { return searchText; }
            set
            {
                try
                {
                    SetProperty(ref searchText, value);

                    if (string.IsNullOrEmpty(value) && IsSearchedTapped)
                    {
                        IsSearchedTapped = false;
                        GetAllNews();
                    }

                    if (!string.IsNullOrEmpty(value) && !string.IsNullOrWhiteSpace(value))
                    {
                        CancelVisible = true;
                    }
                    else
                    {
                        CancelVisible = false;
                    }
                }
                catch (Exception ex)
                {
                    Logger.SendErrorLog(ex);
                }
            }
        }
        private int itemThreshold;
        public int ItemThreshold
        {
            get { return itemThreshold; }
            set { SetProperty(ref itemThreshold, value); }
        }
        private bool isSearchedTapped = false;
        public bool IsSearchedTapped
        {
            get { return isSearchedTapped; }
            set { SetProperty(ref isSearchedTapped, value); }
        }
        private bool cancelVisible = false;
        public bool CancelVisible
        {
            get { return cancelVisible; }
            set { SetProperty(ref cancelVisible, value); }
        }
        #endregion

        #region Commands
        public ICommand NewsSelectCommand { get; set; }
        public Command SearchCommand { get; }
        public Command SearchTapCommand { get; }
        public Command NextNewsCommand { get; }
        public Command ResetCommand { get; }
        #endregion

        #region Constructor
        public NewsViewModel(NewsPage newsPage)
        {
            try
            {
                PageName = "Home";
                IsSearch = IsMenu = true;
                ItemThreshold = 1;
                pg = newsPage;

                NewsSelectCommand = new Command<object>(async news => { MoveToDetails(news); });
                SearchCommand = new Command(OnSearchClicked);
                SearchTapCommand = new Command(OnSearchTapClicked);
                NextNewsCommand = new Command(async news => { await OnNextNewsCommand(news); });
                ResetCommand = new Command(ResetClicked);
            }
            catch (Exception ex)
            {
                Logger.SendErrorLog(ex);
            }
        }
        #endregion

        #region Methods
        public async Task GetAllNews()
        {
            try
            {
                if (IsBusy)
                    return;

                IsBusy = true;

                PageNumber = 0;

                NewsFeeds = new ObservableCollection<NewsResponseData>();

                await OnNextNewsCommand(null);
            }
            catch (Exception ex)
            {
                Logger.SendErrorLog(ex);
            }
            finally
            {
                IsBusy = false;
            }
        }
        public async void MoveToDetails(object obj)
        {
            try
            {
                var data = obj as NewsResponseData;

                if (data != null)
                {
                    await App.NavigationPage.Navigation.PushAsync(new NewsDetailsPage(data));
                }
            }
            catch (Exception ex)
            {
                Logger.SendErrorLog(ex);
            }
        }
        public async Task OnNextNewsCommand(object obj)
        {
            try
            {
                PageNumber++;

                PaginationRequest req = new PaginationRequest();
                req.PageNumber = PageNumber;
                req.PageSize = PageSize;
                req.SearchText = SearchText;

                var res = await newsService.GetAllNews(req);
                if (res != null && res.Count > 0)
                {
                    foreach (var item in res)
                    {
                        if (!string.IsNullOrEmpty(item.picture))
                        {
                            item.NewsPic = Constants.Image_URL + item.picture;
                        }

                        NewsFeeds.Add(item);
                    }

                    if (res.Count < 10)
                    {
                        ItemThreshold = -1;
                    }
                }
                else
                {
                    ItemThreshold = -1;
                }
            }
            catch (Exception ex)
            {
                Logger.SendErrorLog(ex);
            }
        }
        public async void OnSearchClicked(object obj)
        {
            try
            {
                IsSearchedText = !IsSearchedText;

                if (!IsSearchedText)
                {
                    SearchText = string.Empty;

                    if (IsSearchedTapped)
                    {
                        await GetAllNews();
                    }
                }

                if (IsSearchedText)
                {
                    pg.searchEntry.Focus();
                }
            }
            catch (Exception ex)
            {
                Logger.SendErrorLog(ex);
            }
        }
        public async void OnSearchTapClicked(object obj)
        {
            try
            {
                if (string.IsNullOrEmpty(SearchText))
                {
                    UserDialogs.Instance.Toast("Please search something...");
                }
                else
                {
                    IsSearchedTapped = true;
                    await GetAllNews();
                }
            }
            catch (Exception ex)
            {
                Logger.SendErrorLog(ex);
            }
        }

        public void ResetClicked(object obj)
        {
            try
            {
                SearchText = string.Empty;
                
                if (IsSearchedText)
                {
                    pg.searchEntry.Focus();
                }
            }
            catch (Exception ex)
            {
                Logger.SendErrorLog(ex);
            }
        }
        #endregion
    }
}
