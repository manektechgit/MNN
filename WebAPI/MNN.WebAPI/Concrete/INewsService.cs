using Microsoft.AspNetCore.Http;
using MNN.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MNN.WebAPI.Concrete
{
    public interface INewsService
    {
        bool AddNews(News objEvent);
        List<News> GetNewsById(int id);
        bool DeleteNews(int id);
        bool UpdateNews(News newsObj);
        List<News> GetAllNews(int pagesize, int pagenumber, string searchText);
        List<News> GetAllNewsAdmin(int pagesize, int pagenumber, string searchText);
        string UploadFile(IFormFile file);
        bool InsertViews(int NewsId, int MemberId);
        List<NewsUsers> GetNewsViewers(int NewsId);
        List<NewsUsers> GetLikesDislikesUsers(int NewsId, int LikeType);

        bool Changestatus(bool status, int id);
    }
}
