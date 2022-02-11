using MTNews.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Threading.Tasks;

namespace MTNews.Services
{
    public class BackendConnectionImpl : BaseWebService, IBackendConnection
    {
        public BackendConnectionImpl()
        { 
        
        }

        public async Task<ObservableCollection<NewsResponseData>> GetAllNews(PaginationRequest req)
        {
            try
            {
                var str = "?pagesize=" + req.PageSize + "&pagenumber=" + req.PageNumber + "&searchText=" + req.SearchText;

                var res = await GetAsync(Constants.GetAllNews + str);

                if(!string.IsNullOrEmpty(res))
                {
                    var data = JsonConvert.DeserializeObject<ObservableCollection<NewsResponseData>>(res);
                    return data;
                }

                return null;
            }
            catch (Exception ex)
            {
                Logger.SendErrorLog(ex);

                return null;
            }
        }

        public async Task<LoginResponse> Login(LoginRequest login)
        {
            try
            {
                var values = new List<KeyValuePair<string, string>>(){
                        new KeyValuePair<string, string>("userName", login.userName),
                        new KeyValuePair<string, string>("password", login.password)
                    };

                var apiResponse = await AuthorizationAsync(login);

                return apiResponse;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Login Error: " + ex.Message);
                Logger.SendErrorLog(ex);
                return null;
            }
        }
        public async Task<LikeRequest> GetLikeStatus(LikeStatus req)
        {
            try
            {
                var json = JsonConvert.SerializeObject(req);
                var res = await PostAsync(Constants.GetLikeStatus, json);

                if (!string.IsNullOrEmpty(res))
                {
                    var data = JsonConvert.DeserializeObject<LikeRequest> (res);
                    return data;
                }
            }
            catch (Exception ex)
            {
                Logger.SendErrorLog(ex);
            }
            return null;
        }
        public async Task<bool> CreateLikes(LikeRequest req)
        {
            try
            {
                var json = JsonConvert.SerializeObject(req);
                var res = await PostAsync(Constants.CreateLikes, json);

                if (!string.IsNullOrEmpty(res) && res.ToLower() == "true")
                {
                    return true;
                }
            }
            catch (Exception ex)
            {
                Logger.SendErrorLog(ex);
            }
            return false;
        }
        public async Task<bool> UpdateLikes(LikeRequest req)
        {
            try
            {
                var json = JsonConvert.SerializeObject(req);
                var res = await PostAsync(Constants.UpdateLikes, json);

                if (!string.IsNullOrEmpty(res) && res.ToLower() == "true")
                {
                    return true;
                }
            }
            catch (Exception ex)
            {
                Logger.SendErrorLog(ex);
            }
            return false;
        }
        public async Task<ObservableCollection<CommentsResponseData>> GetCommentByID(PaginationRequest req)
        {
            try
            {
                var str = "&pagesize=" + req.PageSize + "&pagenumber=" + req.PageNumber;

                var res = await GetAsync(Constants.GetCommentByID + req.Id + str);

                if (!string.IsNullOrEmpty(res))
                {
                    var data = JsonConvert.DeserializeObject<ObservableCollection<CommentsResponseData>>(res);
                    return data;
                }
            }
            catch (Exception ex)
            {
                Logger.SendErrorLog(ex);
            }
            return null;
        }
        public async Task<bool> CreateComment(CreateCommentRequest req)
        {
            try
            {
                var json = JsonConvert.SerializeObject(req);
                var res = await PostAsync(Constants.CreateComment, json);

                if (!string.IsNullOrEmpty(res) && res.ToLower() == "true")
                {
                    return true;
                }
            }
            catch (Exception ex)
            {
                Logger.SendErrorLog(ex);
            }
            return false;
        }
        public async Task<bool> UpdateComment(CreateCommentRequest req)
        {
            try
            {
                var json = JsonConvert.SerializeObject(req);
                var res = await PostAsync(Constants.UpdateComment, json);

                if (!string.IsNullOrEmpty(res) && res.ToLower() == "true")
                {
                    return true;
                }
            }
            catch (Exception ex)
            {
                Logger.SendErrorLog(ex);
            }
            return false;
        }

        public async Task<bool> DeleteComment(int id)
        {
            try
            {
                var res = await PostAsync(Constants.DeleteComment + id, "");

                if (!string.IsNullOrEmpty(res) && res.ToLower() == "true")
                {
                    return true;
                }
            }
            catch (Exception ex)
            {
                Logger.SendErrorLog(ex);
            }
            return false;
        }

        public async Task<bool> UpdateUser(LoginResponseData req)
        {
            try
            {
                var json = JsonConvert.SerializeObject(req);
                var res = await PostAsync(Constants.UpdateUser, json);

                if (!string.IsNullOrEmpty(res) && res.ToLower() == "true")
                {
                    return true;
                }
            }
            catch (Exception ex)
            {
                Logger.SendErrorLog(ex);
            }
            return false;
        }

        public async Task<bool> InsertViews(int NewsId, int MemberId)
        {
            try
            {
                string url = Constants.InsertViews + NewsId + "&MemberId=" + MemberId;
                var res = await PostAsync(url, "");

                if (!string.IsNullOrEmpty(res) && res.ToLower() == "true")
                {
                    return true;
                }
            }
            catch (Exception ex)
            {
                Logger.SendErrorLog(ex);
            }
            return false;
        }

        public async Task<ObservableCollection<NewsViewers>> GetNewsViewers(int NewsId)
        {
            try
            {
                var res = await GetAsync(Constants.GetNewsViewers + NewsId);

                if (!string.IsNullOrEmpty(res))
                {
                    var data = JsonConvert.DeserializeObject<ObservableCollection<NewsViewers>>(res);
                    return data;
                }
            }
            catch (Exception ex)
            {
                Logger.SendErrorLog(ex);
            }
            return null;
        }

        public async Task<ObservableCollection<NewsViewers>> GetLikesDislikesUsers(int NewsId, int LikeType)
        {
            try
            {
                var res = await GetAsync(Constants.GetLikesDislikesUsers + NewsId + "&LikeType=" + LikeType);

                if (!string.IsNullOrEmpty(res))
                {
                    var data = JsonConvert.DeserializeObject<ObservableCollection<NewsViewers>>(res);
                    return data;
                }
            }
            catch (Exception ex)
            {
                Logger.SendErrorLog(ex);
            }
            return null;
        }

        public async Task<ObservableCollection<NewsResponseData>> GetNewsById(int NewsId)
        {
            try
            {
                var res = await GetAsync(Constants.GetNewsById + NewsId);

                if (!string.IsNullOrEmpty(res))
                {
                    var data = JsonConvert.DeserializeObject<ObservableCollection<NewsResponseData>>(res);
                    return data;
                }
            }
            catch (Exception ex)
            {
                Logger.SendErrorLog(ex);
            }
            return null;
        }
    }
    public class UserNotLoggedInException : Exception
    {
    }
}
