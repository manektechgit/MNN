using Acr.UserDialogs;
using MTNews.Models;
using MTNews.Services;
using MTNews.Views;
using Rg.Plugins.Popup.Pages;
using Rg.Plugins.Popup.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace MTNews
{
    public class NewsDetailsViewModel : BaseViewModel
    {
        #region Properties
        IBackendConnection newsService = DependencyService.Get<IBackendConnection>();
        public int CommentID = 0;
        public LikeRequest likeRequest;
        public int PageNumber = 0;
        public int PageSize = 10;
        public NewsDetailsPage pg;

        private NewsResponseData selectedNews;
        public NewsResponseData SelectedNews
        {
            get { return selectedNews; }
            set { SetProperty(ref selectedNews, value); }
        }
        private ObservableCollection<CommentsResponseData> commentsList;
        public ObservableCollection<CommentsResponseData> CommentsList
        {
            get { return commentsList; }
            set { SetProperty(ref commentsList, value); }
        }
        private ObservableCollection<NewsViewers> usersList;
        public ObservableCollection<NewsViewers> UsersList
        {
            get { return usersList; }
            set { SetProperty(ref usersList, value); }
        }
        private string comment;
        public string Comment
        {
            get { return comment; }
            set { SetProperty(ref comment, value); }
        }
        private string updatedComment;
        public string UpdatedComment
        {
            get { return updatedComment; }
            set { SetProperty(ref updatedComment, value); }
        }
        private string like = "Like";
        public string Like
        {
            get { return like; }
            set { SetProperty(ref like, value); }
        }
        private string disLike = "DisLike";
        public string DisLike
        {
            get { return disLike; }
            set { SetProperty(ref disLike, value); }
        }
        private int itemThreshold;
        public int ItemThreshold
        {
            get { return itemThreshold; }
            set { SetProperty(ref itemThreshold, value); }
        }
        private double commentHeight;
        public double CommentHeight
        {
            get { return commentHeight; }
            set { SetProperty(ref commentHeight, value); }
        }
        private string headerText;
        public string HeaderText
        {
            get { return headerText; }
            set { SetProperty(ref headerText, value); }
        }
        private double usersHeight;
        public double UsersHeight
        {
            get { return usersHeight; }
            set { SetProperty(ref usersHeight, value); }
        }
        private bool isComment;
        public bool IsComment
        {
            get { return isComment; }
            set { SetProperty(ref isComment, value); }
        }
        private string webviewContent;
        public string WebviewContent
        {
            get { return webviewContent; }
            set { SetProperty(ref webviewContent, value); }
        }
        private string newsPicture;
        public string NewsPicture
        {
            get { return newsPicture; }
            set { SetProperty(ref newsPicture, value); }
        }
        #endregion

        #region Commands
        public Command LikeCommand { get; }
        public Command DisLikeCommand { get; }
        public Command SendCommand { get; }
        public Command UpdateCommand { get; }
        public Command CancelCommand { get; }
        public Command NextCommentCommand { get; }
        public ICommand ViewsCommand { get; set; }
        public ICommand CommentsCommand { get; set; }

        #endregion

        #region Constructor
        public NewsDetailsViewModel(NewsResponseData newss, NewsDetailsPage detailsPage)
        {
            try
            {
                PageName = "News Detail";
                IsMenu = true;

                ItemThreshold = 1;
                SelectedNews = newss;
                NewsPicture = newss.NewsPic;
                pg = detailsPage;

                LikeCommand = new Command(OnLikeClicked);
                DisLikeCommand = new Command(OnDisLikeClicked);
                SendCommand = new Command(OnSendClicked);
                UpdateCommand = new Command(OnUpdateClicked);
                CancelCommand = new Command(OnCancelClicked);

                NextCommentCommand = new Command(async comment => { await OnNextCommentCommand(comment); });
                ViewsCommand = new Command<object>(async news => { OpenUserPopup(news); });
                CommentsCommand = new Command<object>(async news => { OpenComments(news); });

                //var content = newss.content.Replace("\"", "'").Replace("&ldquo;", string.Empty).Replace("&rdquo;", string.Empty).Replace("&rsquo;", string.Empty);
                //WebviewContent = new HtmlWebViewSource()
                //{
                //    Html = "<!DOCTYPE html><html><body>" + content + "</body></html>"
                //};

                WebviewContent = newss.content;
            }
            catch (Exception ex)
            {
                Logger.SendErrorLog(ex);
            }
        }
        #endregion

        #region Methods
        public async void OnLikeClicked(object obj)
        {
            try
            {
                if (Like == "Like")
                {
                    Like = "LikeFill";
                    await UpdateLikeStatus(1);
                }
                else
                {
                    Like = "Like";
                    await UpdateLikeStatus(0);
                }
            }
            catch (Exception ex)
            {
                Logger.SendErrorLog(ex);
            }
        }
        public async void OnDisLikeClicked(object obj)
        {
            try
            {
                if (DisLike == "DisLike")
                {
                    DisLike = "DisLikeFill";
                    await UpdateLikeStatus(2);
                }
                else
                {
                    DisLike = "DisLike";
                    await UpdateLikeStatus(0);
                }
            }
            catch (Exception ex)
            {
                Logger.SendErrorLog(ex);
            }
        }
        public async Task GetAllComments()
        {
            try
            {
                if (IsBusy)
                    return;

                IsBusy = true;

                PageNumber = 0;

                CommentsList = new ObservableCollection<CommentsResponseData>();

                await OnNextCommentCommand(null);
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
        public async Task OnNextCommentCommand(object obj)
        {
            try
            {
                PageNumber++;

                PaginationRequest req = new PaginationRequest();
                req.PageNumber = PageNumber;
                req.PageSize = PageSize;
                req.Id = SelectedNews.id;

                var res = await newsService.GetCommentByID(req);
                if (res != null && res.Count > 0)
                {
                    foreach (var item in res)
                    {
                        if (item.userId == MemberId)
                        {
                            item.IsMenuVisible = true;

                            if (!string.IsNullOrEmpty(item.comment))
                            {
                                if (item.comment.Length > 50)
                                {
                                    item.IsCount = (item.comment.Length / 50);
                                }
                            }
                            item.ProfilePic = UserProfilePic;
                        }
                        else
                        {
                            item.ProfilePic = "";
                        }

                        if (!string.IsNullOrEmpty(item.UserName))
                        {
                            item.ShortName = Utilities.GetShortName(item.UserName);
                        }

                        CommentsList.Add(item);
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

                UpdateHeight();
            }
            catch (Exception ex)
            {
                Logger.SendErrorLog(ex);
            }
        }
        public void UpdateHeight()
        {
            try
            {
                if (CommentsList != null && CommentsList.Count > 0)
                {
                    int height = 0;
                    foreach (var item in CommentsList)
                    {
                        if (item.IsCount > 1)
                            height += item.IsCount;
                    }

                    CommentHeight = (CommentsList.Count * 100) + 50;

                    if (height > 0)
                    {
                        CommentHeight = CommentHeight + (height * 20);
                    }
                }
                else
                {
                    CommentHeight = 100;
                }
            }
            catch (Exception ex)
            {
                Logger.SendErrorLog(ex);
            }
        }
        public async void OnSendClicked(object obj)
        {
            try
            {
                if (string.IsNullOrEmpty(Comment))
                {
                    UserDialogs.Instance.Toast("Please write comment");
                }
                else
                {
                    await CreateComments();
                }
            }
            catch (Exception ex)
            {
                Logger.SendErrorLog(ex);
            }
        }
        public async Task CreateComments()
        {
            try
            {
                UserDialogs.Instance.ShowLoading();

                var req = new CreateCommentRequest();
                req.userId = MemberId;
                req.comment = Comment;
                req.isActive = true;
                req.newsFeedId = SelectedNews.id;
                req.createdDate = DateTime.Now;
                req.createdBy = MemberId;
                req.modifiedDate = DateTime.Now;
                req.modifiedBy = MemberId;

                var res = await newsService.CreateComment(req);
                if (res)
                {
                    UserDialogs.Instance.Toast("Comment added");
                    Comment = string.Empty;

                    await GetAfterCreate();
                    await GetNewsById();
                }

                UserDialogs.Instance.HideLoading();
            }
            catch (Exception ex)
            {
                Logger.SendErrorLog(ex);
                UserDialogs.Instance.HideLoading();
            }
        }
        public async void OpenPopup()
        {
            try
            {
                PopupPage page;
                page = new EditPopupPage();
                page.BindingContext = this;

                await PopupNavigation.Instance.PushAsync(page);
                page.CloseWhenBackgroundIsClicked = false;
            }
            catch (Exception ex)
            {
                Logger.SendErrorLog(ex);
            }
        }
        public async void OnCancelClicked(object obj)
        {
            try
            {
                await PopupNavigation.Instance.PopAsync(true);

                CommentID = 0;
            }
            catch (Exception ex)
            {
                Logger.SendErrorLog(ex);
            }
        }
        public async Task UpdateComments()
        {
            try
            {
                UserDialogs.Instance.ShowLoading();

                var req = new CreateCommentRequest();
                req.id = CommentID;
                req.userId = MemberId;
                req.comment = UpdatedComment;
                req.isActive = true;
                req.newsFeedId = SelectedNews.id;
                req.createdDate = DateTime.Now;
                req.createdBy = MemberId;
                req.modifiedDate = DateTime.Now;
                req.modifiedBy = MemberId;

                var res = await newsService.UpdateComment(req);
                if (res)
                {
                    UserDialogs.Instance.Toast("Comment updated");

                    var item = CommentsList.Where(x => x.id == CommentID)?.FirstOrDefault();
                    if (item != null)
                    {
                        item.comment = UpdatedComment;
                    }

                    UpdatedComment = Comment = string.Empty;
                }

                OnCancelClicked(null);

                UserDialogs.Instance.HideLoading();
            }
            catch (Exception ex)
            {
                OnCancelClicked(null);

                Logger.SendErrorLog(ex);
                UserDialogs.Instance.HideLoading();
            }
        }
        public async void OnUpdateClicked(object obj)
        {
            try
            {
                if (string.IsNullOrEmpty(UpdatedComment))
                {
                    UserDialogs.Instance.Toast("Please write comment");
                }
                else
                {
                    await UpdateComments();
                }
            }
            catch (Exception ex)
            {
                Logger.SendErrorLog(ex);
            }
        }
        public async Task DeleteComments(CommentsResponseData item)
        {
            try
            {
                var ress = await UserDialogs.Instance.ConfirmAsync("Are you sure you want to delete Comment?", "Comment", "Yes", "No");
                if (ress)
                {
                    UserDialogs.Instance.ShowLoading();

                    var res = await newsService.DeleteComment(item.id);
                    if (res)
                    {
                        UserDialogs.Instance.Toast("Comment deleted");
                        Comment = string.Empty;

                        CommentsList.Remove(item);

                        UpdateHeight();
                    }

                    UserDialogs.Instance.HideLoading();
                }
            }
            catch (Exception ex)
            {
                Logger.SendErrorLog(ex);
                UserDialogs.Instance.HideLoading();
            }
        }
        public async Task GetLikeStatus()
        {
            try
            {
                UserDialogs.Instance.ShowLoading();

                LikeStatus req = new LikeStatus();
                req.newsFeedId = SelectedNews.id;
                req.userId = MemberId;

                var res = await newsService.GetLikeStatus(req);
                if (res != null)
                {
                    likeRequest = res;
                    if (res.isLiked == 1)//Liked news
                    {
                        Like = "LikeFill";
                        DisLike = "DisLike";
                    }
                    else if(res.isLiked == 2)//DisLiked news
                    {
                        Like = "Like";
                        DisLike = "DisLikeFill";
                    }
                    else
                    {
                        Like = "Like";
                        DisLike = "DisLike";
                    }
                }

                UserDialogs.Instance.HideLoading();
            }
            catch (Exception ex)
            {
                Logger.SendErrorLog(ex);
                UserDialogs.Instance.HideLoading();
            }
        }
        public async Task UpdateLikeStatus(int like)
        {
            try
            {
                UserDialogs.Instance.ShowLoading();

                var res = false;

                LikeRequest req = new LikeRequest();
                req.id = 0;
                req.newsFeedId = SelectedNews.id;
                req.userId = MemberId;
                req.isLiked = like;
                req.likedDate = DateTime.Now;

                if (likeRequest != null && likeRequest.userId != 0 && likeRequest.newsFeedId != 0)
                {
                    req.isLiked = like;
                    res = await newsService.UpdateLikes(req);
                }
                else
                {
                    res = await newsService.CreateLikes(req);
                }

                if (res)
                {
                    await GetLikeStatus();
                    await GetNewsById();
                }

                UserDialogs.Instance.HideLoading();
            }
            catch (Exception ex)
            {
                Logger.SendErrorLog(ex);
                UserDialogs.Instance.HideLoading();
            }
        }
        public async void OpenUserPopup(object obj)
        {
            try
            {
                var data = obj as string;

                if (!string.IsNullOrEmpty(data))
                {
                    UserDialogs.Instance.ShowLoading();
                    
                    UsersHeight = 0;
                    UsersList = new ObservableCollection<NewsViewers>();

                    if (data == "1")
                    {
                        UsersList = await newsService.GetNewsViewers(SelectedNews.id);
                        HeaderText = "Viewers";
                    }
                    else if (data == "2")
                    {
                        UsersList = await newsService.GetLikesDislikesUsers(SelectedNews.id, 1);
                        HeaderText = "Likes";
                    }
                    else if (data == "3")
                    {
                        UsersList = await newsService.GetLikesDislikesUsers(SelectedNews.id, 2);
                        HeaderText = "Dislikes";
                    }
                    
                    UserDialogs.Instance.HideLoading();

                    if (UsersList != null && UsersList.Count > 0)
                    {
                        //var popupTasks = PopupNavigation.Instance.PopupStack.ToList().Select(page => PopupNavigation.Instance.RemovePageAsync(page, false));
                        //return Task.WhenAll(popupTasks);

                        PopupPage page;
                        page = new UsersPopup();
                        page.BindingContext = this;
                        await PopupNavigation.Instance.PushAsync(page);
                        page.CloseWhenBackgroundIsClicked = false;

                        UsersHeight = UsersList.Count * 60;

                        foreach (var item in UsersList)
                        {
                            item.ShortName = Utilities.GetShortName(item.userName);
                        }
                    }
                    else
                    {
                        if (data == "2")
                            UserDialogs.Instance.Toast("No Likes yet.");
                        if (data == "3")
                            UserDialogs.Instance.Toast("No Dislikes yet.");
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.SendErrorLog(ex);
            }
        }
        public async void OpenComments(object obj)
        {
            try
            {
                UserDialogs.Instance.ShowLoading();
                IsComment = !IsComment;

                if (IsComment)
                {
                   await pg.ScrollToComments();
                }
                UserDialogs.Instance.HideLoading();
            }
            catch (Exception ex)
            {
                Logger.SendErrorLog(ex);
                UserDialogs.Instance.HideLoading();
            }
        }
        public async Task UpdateNewsViews()
        {
            try
            {
                var res = await newsService.InsertViews(SelectedNews.id, MemberId);
            }
            catch (Exception ex)
            {
                Logger.SendErrorLog(ex);
            }
        }
        public async Task GetAfterCreate()
        {
            try
            {
                if (IsBusy)
                    return;

                IsBusy = true;
                                
                UserDialogs.Instance.ShowLoading();

                var pgNo = 1;
                var pgSize = 1;

                if (CommentsList != null && CommentsList.Count > 0)
                {
                    pgSize = CommentsList.Count;
                    pgNo = 2;
                }
                else
                {
                    CommentsList = new ObservableCollection<CommentsResponseData>();
                }

                PaginationRequest req = new PaginationRequest();
                req.PageNumber = pgNo;
                req.PageSize = pgSize;
                req.Id = SelectedNews.id;

                var res = await newsService.GetCommentByID(req);
                if (res != null && res.Count > 0)
                {
                    foreach (var item in res)
                    {
                        if (item.userId == MemberId)
                        {
                            item.IsMenuVisible = true;

                            if (!string.IsNullOrEmpty(item.comment))
                            {
                                if (item.comment.Length > 50)
                                {
                                    item.IsCount = (item.comment.Length / 50);
                                }
                            }
                        }

                        CommentsList.Add(item);
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

                UpdateHeight();

                UserDialogs.Instance.HideLoading();
            }
            catch (Exception ex)
            {
                Logger.SendErrorLog(ex);
                UserDialogs.Instance.HideLoading();
            }
            finally
            {
                IsBusy = false;
            }
        }
        public async Task GetNewsById()
        {
            try
            {
                var res = await newsService.GetNewsById(SelectedNews.id);
                if (res != null && res.Count > 0)
                {
                    if (!string.IsNullOrEmpty(res[0].picture))
                    {
                        res[0].NewsPic = Constants.Image_URL + res[0].picture;
                    }

                    SelectedNews = res[0];
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