using MTNews.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Threading.Tasks;

namespace MTNews.Services
{
    public interface IBackendConnection
    {
        Task<LoginResponse> Login(LoginRequest login);
        Task<bool> UpdateUser(LoginResponseData req);
        Task<ObservableCollection<NewsResponseData>> GetAllNews(PaginationRequest req);
        Task<LikeRequest> GetLikeStatus(LikeStatus req);
        Task<bool> CreateLikes(LikeRequest req);
        Task<bool> UpdateLikes(LikeRequest req);
        Task<ObservableCollection<CommentsResponseData>> GetCommentByID(PaginationRequest req);
        Task<bool> CreateComment(CreateCommentRequest req); 
        Task<bool> UpdateComment(CreateCommentRequest req);
        Task<bool> DeleteComment(int id);
        Task<bool> InsertViews(int NewsId, int MemberId);
        Task<ObservableCollection<NewsViewers>> GetNewsViewers(int NewsId);
        Task<ObservableCollection<NewsViewers>> GetLikesDislikesUsers(int NewsId, int LikeType);
        Task<ObservableCollection<NewsResponseData>> GetNewsById(int NewsId);
    }
}
