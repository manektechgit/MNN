using Microsoft.Extensions.Configuration;
using MNN.Data.Models;
using MNN.WebAPI.Concrete;
using MNN.WebAPI.Helpers;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace MNN.WebAPI.Services
{
    public class UserLikesService: IUserLikesService
    {
        private readonly IConfiguration _configuration;

        public UserLikesService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public bool AddLikes(UserLikes objUserLikes)
        {
            try
            {
                SqlParameter[] prms = new SqlParameter[5];

                prms[0] = new SqlParameter("@userId", objUserLikes.UserId);
                prms[1] = new SqlParameter("@isLiked", (objUserLikes.IsLiked)); 
                prms[2] = new SqlParameter("@newsFeedId", objUserLikes.NewsFeedId);
                prms[3] = new SqlParameter("@likedDate", objUserLikes.LikedDate);  
                prms[4] = new SqlParameter("@type", "Insert");

                (new DBHelpers(_configuration).ExecuteNonQuery)("sp_Like", prms);
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
        public bool UpdateLikes(UserLikes objUserLikes)
        {
            try
            {
                SqlParameter[] prms = new SqlParameter[6];
                prms[0] = new SqlParameter("@id", objUserLikes.Id);
                prms[1] = new SqlParameter("@userId", objUserLikes.UserId);
                prms[2] = new SqlParameter("@isLiked", (objUserLikes.IsLiked));
                prms[3] = new SqlParameter("@newsFeedId", objUserLikes.NewsFeedId);
                prms[4] = new SqlParameter("@likedDate", objUserLikes.LikedDate);
                prms[5] = new SqlParameter("@type", "Update");

                (new DBHelpers(_configuration).ExecuteNonQuery)("sp_Like", prms);
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
        public UserLikes GetLikes(GetUserLikes objUserLikes)
        {
            try
            {
                UserLikes Likes = new UserLikes();
               SqlParameter[] prms = new SqlParameter[3];
                prms[0] = new SqlParameter("@newsFeedId", objUserLikes.NewsFeedId);
                prms[1] = new SqlParameter("@userId", objUserLikes.UserId);
                prms[2] = new SqlParameter("@type", "GetUserLike");

                DataTable dt = (new DBHelpers(_configuration).GetTableRow)("sp_Like", prms);
                if (dt != null)
                {
                    if (dt.Rows.Count > 0)
                    {
                        Likes.UserId = (long)dt.Rows[0]["MemberID"];
                        Likes.NewsFeedId = (long)dt.Rows[0]["NewsID"];
                        Likes.IsLiked = (int)dt.Rows[0]["Liked"];
                        Likes.LikedDate = (DateTime?)dt.Rows[0]["CreatedDate"];
                    }
                }

                return Likes;
            }
            catch (Exception ex)
            {
                return null;
            }
        }
    }
}
