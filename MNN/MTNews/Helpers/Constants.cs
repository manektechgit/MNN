using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace MTNews
{
    public static class Constants
    {
        public const string AppName = "MTNews";
        public const string Error = "Error";
        public const string Ok = "OK";
        public const string ServerUrl = "";
        
        public const string Authenticate_URL = "http://mtworks.manektech.com/DataService.asmx/AuthenticateUser";
        public const string BASE_URL = "http://50.21.182.225/mtnwebapi/";

        public const string StatusCode = "200";
        public const string ServerError = "Internal Server Error";

        public static readonly Regex CheckValidEmail = new Regex(@"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$");

        public const string IsLoggedIn = "IsLoggedIn";
        public const string MemberID = "MemberID";
        public const string UserJson = "UserJson";

        public const string Image_URL = BASE_URL + "images/";

        public const string GetAllNews = "/News/GetAllNews";

        public const string GetLikeStatus = "/UserLikes/getLikes";
        public const string CreateLikes = "/UserLikes/createLikes";
        public const string UpdateLikes = "/UserLikes/updateLikes";


        public const string GetCommentByID = "/Comments/GetCommentById?id=";
        public const string CreateComment = "/Comments/createComment";
        public const string UpdateComment = "/Comments/updateComment";
        public const string DeleteComment = "/Comments/deleteComment?id=";

        public const string UpdateUser = "/Users/updateUser";

        public const string InsertViews = "/News/InsertViews?NewsId=";
        public const string GetNewsViewers = "/News/GetNewsViewers?NewsId=";
        public const string GetLikesDislikesUsers = "/News/GetLikesDislikesUsers?NewsId=";
        public const string GetNewsById = "/News/GetNewsById?id=";
    }

    public static class Utilities
    {
        public static string GetShortName(string name)
        {
            var shortName = string.Empty;
            try
            {
                if (!string.IsNullOrEmpty(name))
                {
                    var items = name.Split(' ');
                    if (items.Count() == 1)
                    {
                        if (name.Length == 1)
                            shortName = name.Substring(0, 1);
                        else
                            shortName = name.Substring(0, 2);
                    }
                    else if (items.Count() >= 2)
                    {
                        shortName = items[0].Substring(0, 1) + items[1].Substring(0, 1);
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.SendErrorLog(ex);
            }

            return shortName;
        }
    }
}