using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using MNN.Data.Models;
using MNN.WebAPI.Concrete;
using MNN.WebAPI.Helpers;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace MNN.WebAPI.Services
{
    public class NewsService: INewsService
    {
        private readonly IConfiguration _configuration;
        private readonly IHostingEnvironment _environment;
        
        public NewsService(IConfiguration configuration, IHostingEnvironment environment)
        {
            _configuration = configuration;
            _environment = environment;
        }
        public bool AddNews(News newsObj)
        {
            try
            {
                long size = Convert.ToInt64(newsObj.Image.fileSize);
                if (size > 0)
                {
                    ConvertToBase64Image(newsObj.Image);
                    UploadImage(newsObj.Image, newsObj.Image.fileName);
                }
                SqlParameter[] prms = new SqlParameter[9];
                Byte[] Emptyarray = new Byte[64];
                prms[0] = new SqlParameter("@title", newsObj.Title);

                if (!string.IsNullOrWhiteSpace(Convert.ToString(newsObj.Image.fileName)))
                {
                    prms[1] = new SqlParameter("@picture", newsObj.Image.fileName);
                }
                else
                {
                    prms[1] = new SqlParameter("@picture", DBNull.Value);
                }
               // prms[1] = new SqlParameter("@picture", newsObj.Picture);
                 
                prms[2] = new SqlParameter("@isActive", newsObj.IsActive);
                prms[3] = new SqlParameter("@content", newsObj.Content);

                if (!string.IsNullOrWhiteSpace(Convert.ToString(newsObj.CreatedDate)))
                {
                    prms[4] = new SqlParameter("@createdDate", DateTime.Now);
                }
                else
                {
                    prms[4] = new SqlParameter("@createdDate", DateTime.Now);
                }

                //prms[4] = new SqlParameter("@createdDate", newsObj.CreatedDate);
                prms[5] = new SqlParameter("@createdBy", newsObj.CreatedBy);
                if (!string.IsNullOrWhiteSpace(Convert.ToString(newsObj.ModifiedDate)))
                {
                    prms[6] = new SqlParameter("@modifiedDate", newsObj.ModifiedDate);
                }
                else
                {
                    prms[6] = new SqlParameter("@modifiedDate", DBNull.Value);
                }

               //prms[6] = new SqlParameter("@modifiedDate", newsObj.ModifiedDate);
                prms[7] = new SqlParameter("@modifiedBy", newsObj.ModifiedBy);
                prms[8] = new SqlParameter("@type", "Insert");
                
                (new DBHelpers(_configuration).ExecuteNonQuery)("sp_News", prms);
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public bool UpdateNews(News newsObj)
        {
            try
            {
                if(newsObj.Image != null)
                {
                    long size = Convert.ToInt64(newsObj.Image.fileSize);
                    if (size > 0)
                    {
                        ConvertToBase64Image(newsObj.Image);
                        UploadImage(newsObj.Image, newsObj.Image.fileName);
                    }
                }
                
                SqlParameter[] prms = new SqlParameter[9];
                prms[0] = new SqlParameter("@title", newsObj.Title);
                // prms[1] = new SqlParameter("@picture", newsObj.Picture);
                if (newsObj.Image == null)
                {
                    prms[1] = new SqlParameter("@picture", newsObj.Picture);
                  
                }
                else if(newsObj.Image != null)
                {
                    if (!string.IsNullOrWhiteSpace(Convert.ToString(newsObj.Image.fileName)))
                    {
                        prms[1] = new SqlParameter("@picture", newsObj.Image.fileName);
                    }
                    else
                    {
                        prms[1] = new SqlParameter("@picture", DBNull.Value);
                    }
                }
                else
                {
                    prms[1] = new SqlParameter("@picture", DBNull.Value);
                }
               

                prms[2] = new SqlParameter("@isActive", newsObj.IsActive);
                prms[3] = new SqlParameter("@content", newsObj.Content);
                if (!string.IsNullOrWhiteSpace(Convert.ToString(newsObj.CreatedDate)))
                {
                    prms[4] = new SqlParameter("@createdDate", newsObj.CreatedDate);
                }
              
                //prms[4] = new SqlParameter("@createdDate", newsObj.CreatedDate);
               // prms[5] = new SqlParameter("@modifiedDate", newsObj.ModifiedDate);
                if (!string.IsNullOrWhiteSpace(Convert.ToString(newsObj.ModifiedDate)))
                {
                    prms[5] = new SqlParameter("@modifiedDate", newsObj.ModifiedDate);
                }
                else
                {
                    prms[5] = new SqlParameter("@modifiedDate", DateTime.Now);
                }
                prms[6] = new SqlParameter("@modifiedBy", newsObj.ModifiedBy);  
                prms[7] = new SqlParameter("@id", newsObj.Id);
                prms[8] = new SqlParameter("@type", "Update");
                
                (new DBHelpers(_configuration).ExecuteNonQuery)("sp_News", prms);
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public bool DeleteNews(int id)
        {
            try
            {
                SqlParameter[] prms = new SqlParameter[2];
                prms[0] = new SqlParameter("@id", id);
                prms[1] = new SqlParameter("@type", "Delete");
                (new DBHelpers(_configuration).ExecuteNonQuery)("sp_News", prms);
                return true;
            } 
            catch(Exception ex)
            {
                return false;
            }
        }

        public bool Changestatus(bool status , int id)
        {
            try
            {
                SqlParameter[] prms = new SqlParameter[3];
                prms[0] = new SqlParameter("@id", id);
                prms[1] = new SqlParameter("@Status", status);
                prms[2] = new SqlParameter("@type", "ChangeStatus");
                (new DBHelpers(_configuration).ExecuteNonQuery)("sp_AdminNews", prms);
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public List<News> GetNewsById(int id)
        {
            List<News> listObjNews = new List<News>();
            SqlParameter[] prms = new SqlParameter[2];
            prms[0] = new SqlParameter("@type", "GetNewsById");
            prms[1] = new SqlParameter("@id", id);
            DataTable dt = (new DBHelpers(_configuration).GetTableRow)("sp_News", prms);
            if (dt != null)
            {
                if (dt.Rows.Count > 0)
                {
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        News newsobj = new News();
                        newsobj.Id = Convert.ToInt32(dt.Rows[i]["Id"]);
                        newsobj.Title = Convert.ToString(dt.Rows[i]["Title"]);
                        newsobj.Content = Convert.ToString(dt.Rows[i]["Content"]);
                        newsobj.NumberOfLikes = Convert.ToInt32(dt.Rows[i]["NumberOfLikes"]);
                        newsobj.NumberOfDisLikes = Convert.ToInt32(dt.Rows[i]["NumberOfDisLikes"]);
                        newsobj.NumberOfViews = Convert.ToInt32(dt.Rows[i]["NumberOfViews"]);
                        newsobj.NumberOfComments = Convert.ToInt32(dt.Rows[i]["NumberOfComments"]);

                        if (!string.IsNullOrWhiteSpace(Convert.ToString(dt.Rows[0]["Picture"])))
                        {
                            newsobj.Picture = Convert.ToString(dt.Rows[0]["Picture"]);
                        }
                        if (!string.IsNullOrWhiteSpace(Convert.ToString(dt.Rows[i]["CreatedDate"])))
                        {
                            newsobj.CreatedDate = Convert.ToDateTime(dt.Rows[i]["CreatedDate"]);
                        }
                        newsobj.CreatedBy = Convert.ToInt32(dt.Rows[i]["CreatedBy"]);
                        newsobj.ModifiedBy = Convert.ToInt32(dt.Rows[i]["ModifiedBy"]);
                        if (!string.IsNullOrWhiteSpace(Convert.ToString(dt.Rows[i]["ModifiedDate"])))
                        {
                            newsobj.ModifiedDate = Convert.ToDateTime(dt.Rows[i]["ModifiedDate"]);
                        }
                        listObjNews.Add(newsobj);
                    }
                }
            }
            return listObjNews;
        }
        public List<News> GetAllNews(int pagesize, int pagenumber, string searchText)
        {
            List<News> listObjNews = new List<News>();

            SqlParameter[] prms = new SqlParameter[4];
            prms[0] = new SqlParameter("@type", "GetAllNews");
            prms[1] = new SqlParameter("@searchText", searchText);
            prms[2] = new SqlParameter("@pagenumber", pagenumber);
            prms[3] = new SqlParameter("@pagesize", pagesize);

            DataSet ds = (new DBHelpers(_configuration).GetDatasetFromSP)("sp_News", prms);
            if (ds.Tables != null)
            {
                if (ds.Tables[0].Rows.Count > 0)
                {
                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        News newsobj = new News();
                        newsobj.Id = Convert.ToInt32(ds.Tables[0].Rows[i]["Id"]);
                        newsobj.Title = Convert.ToString(ds.Tables[0].Rows[i]["Title"]);
                        newsobj.Content = Convert.ToString(ds.Tables[0].Rows[i]["Content"]);
                        newsobj.NumberOfLikes = Convert.ToInt32(ds.Tables[0].Rows[i]["NumberOfLikes"]);
                        newsobj.NumberOfDisLikes = Convert.ToInt32(ds.Tables[0].Rows[i]["NumberOfDisLikes"]);
                        newsobj.NumberOfViews = Convert.ToInt32(ds.Tables[0].Rows[i]["NumberOfViews"]);
                        newsobj.NumberOfComments = Convert.ToInt32(ds.Tables[0].Rows[i]["NumberOfComments"]);
                      //  newsobj.IsActive = Convert.ToBoolean(ds.Tables[0].Rows[i]["IsActive"]);

                        if (!string.IsNullOrWhiteSpace(Convert.ToString(ds.Tables[0].Rows[i]["Picture"])))
                        {
                            newsobj.Picture = ds.Tables[0].Rows[i]["Picture"].ToString();
                        }
                        if (!string.IsNullOrWhiteSpace(Convert.ToString(ds.Tables[0].Rows[i]["CreatedDate"])))
                        {
                            newsobj.CreatedDate = Convert.ToDateTime(ds.Tables[0].Rows[i]["CreatedDate"]);
                        }
                        //newsobj.CreatedDate = Convert.ToDateTime(dt.Rows[i]["CreatedDate"]);
                        newsobj.CreatedBy = Convert.ToInt32(ds.Tables[0].Rows[i]["CreatedBy"]);
                        newsobj.ModifiedBy = Convert.ToInt32(ds.Tables[0].Rows[i]["ModifiedBy"]);
                        if (!string.IsNullOrWhiteSpace(Convert.ToString(ds.Tables[0].Rows[i]["ModifiedDate"])))
                        {
                            newsobj.ModifiedDate = Convert.ToDateTime(ds.Tables[0].Rows[i]["ModifiedDate"]);
                        }

                        //DataRow[] dr = ds.Tables[1].Select("NEWSID = '" + ds.Tables[0].Rows[i]["Id"] + "'");
                        //if (dr != null && dr.Length > 0)
                        //{
                        //    newsobj.NumberOfComments = Convert.ToInt32(dr[0]["NumberOfComments"]);
                        //}
                        //else
                        //{
                        //    newsobj.NumberOfComments = 0;
                        //}
                        //DataRow[] dr1 = ds.Tables[2].Select("NEWSID = '" + ds.Tables[0].Rows[i]["Id"] + "'");
                        //if (dr1 != null && dr1.Length > 0)
                        //{
                        //    newsobj.NumberOfLikes = Convert.ToInt32(dr1[0]["NumberOfLikes"]);
                        //}
                        //else
                        //{
                        //    newsobj.NumberOfLikes = 0;
                        //}

                        listObjNews.Add(newsobj);
                    }
                }
            }



            return listObjNews;
        }
        public List<News> GetAllNewsAdmin(int pagesize,int pagenumber,string searchText)
        {
            List<News> listObjNews = new List<News>();
         
            SqlParameter[] prms = new SqlParameter[5];
            prms[0] = new SqlParameter("@type", "GetAllNews");
            prms[1] = new SqlParameter("@searchText", searchText);
            prms[2] = new SqlParameter("@pagenumber", pagenumber);
            prms[3] = new SqlParameter("@pagesize", pagesize);
            prms[4] = new SqlParameter("@Status",false);

            DataSet ds = (new DBHelpers(_configuration).GetDatasetFromSP)("sp_AdminNews", prms);
            if (ds.Tables != null)
            {
                if (ds.Tables[0].Rows.Count > 0)
                {
                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        News newsobj = new News();
                        newsobj.Id = Convert.ToInt32(ds.Tables[0].Rows[i]["Id"]);
                        newsobj.Title = Convert.ToString(ds.Tables[0].Rows[i]["Title"]);
                        newsobj.Content = Convert.ToString(ds.Tables[0].Rows[i]["Content"]);
                        newsobj.NumberOfLikes = Convert.ToInt32(ds.Tables[0].Rows[i]["NumberOfLikes"]);
                        newsobj.NumberOfDisLikes = Convert.ToInt32(ds.Tables[0].Rows[i]["NumberOfDisLikes"]);
                        newsobj.NumberOfViews = Convert.ToInt32(ds.Tables[0].Rows[i]["NumberOfViews"]);
                        newsobj.NumberOfComments = Convert.ToInt32(ds.Tables[0].Rows[i]["NumberOfComments"]);
                        newsobj.IsActive = Convert.ToBoolean(ds.Tables[0].Rows[i]["IsActive"]);

                        if (!string.IsNullOrWhiteSpace(Convert.ToString(ds.Tables[0].Rows[i]["Picture"])))
                        {
                            newsobj.Picture = ds.Tables[0].Rows[i]["Picture"].ToString();
                        }
                        if (!string.IsNullOrWhiteSpace(Convert.ToString(ds.Tables[0].Rows[i]["CreatedDate"])))
                        {
                            newsobj.CreatedDate = Convert.ToDateTime(ds.Tables[0].Rows[i]["CreatedDate"]);
                        }
                        //newsobj.CreatedDate = Convert.ToDateTime(dt.Rows[i]["CreatedDate"]);
                        newsobj.CreatedBy = Convert.ToInt32(ds.Tables[0].Rows[i]["CreatedBy"]);
                        newsobj.ModifiedBy = Convert.ToInt32(ds.Tables[0].Rows[i]["ModifiedBy"]);
                        if (!string.IsNullOrWhiteSpace(Convert.ToString(ds.Tables[0].Rows[i]["ModifiedDate"])))
                        {
                            newsobj.ModifiedDate = Convert.ToDateTime(ds.Tables[0].Rows[i]["ModifiedDate"]);
                        }

                        //DataRow[] dr = ds.Tables[1].Select("NEWSID = '" + ds.Tables[0].Rows[i]["Id"] + "'");
                        //if (dr != null && dr.Length > 0)
                        //{
                        //    newsobj.NumberOfComments = Convert.ToInt32(dr[0]["NumberOfComments"]);
                        //}
                        //else
                        //{
                        //    newsobj.NumberOfComments = 0;
                        //}
                        //DataRow[] dr1 = ds.Tables[2].Select("NEWSID = '" + ds.Tables[0].Rows[i]["Id"] + "'");
                        //if (dr1 != null && dr1.Length > 0)
                        //{
                        //    newsobj.NumberOfLikes = Convert.ToInt32(dr1[0]["NumberOfLikes"]);
                        //}
                        //else
                        //{
                        //    newsobj.NumberOfLikes = 0;
                        //}
                        
                        listObjNews.Add(newsobj);
                    }
                }
            }



            return listObjNews;
        }

        public string UploadFile(IFormFile file)
        {

            try
            {
                if (file == null || file.Length == 0)
                    return "";
                string filePathWithoutExt = Path.GetFileNameWithoutExtension(file.FileName);
                var filePath = _environment.WebRootPath + @"\images";
                if (!Directory.Exists(filePath))
                {
                    Directory.CreateDirectory(filePath);
                }

                string filename = string.Format("img-{0:yyyy-MM-dd_hh-mm-ss-tt}.png", DateTime.Now);

                using (var fileStream = new FileStream(Path.Combine(filePath, filename), FileMode.Create))
                {
                    file.CopyToAsync(fileStream);
                }


                return filename;
            }
            catch (Exception ex)
            {

                return "";
            }
        }
        public void ConvertToBase64Image(FileToUpload file)
        {
            if (file != null && file.fileAsBase64.Contains(","))
            {
                file.fileAsBase64 = file.fileAsBase64.Split(",")[1];
                file.FileAsByteArray = Convert.FromBase64String(file.fileAsBase64);
            }
        }
        public void UploadImage(FileToUpload filetoUpload, string fileName)
        {
            if (filetoUpload != null)
            {
                var filePath = _environment.WebRootPath + @"\images";
                if (!Directory.Exists(filePath))
                {
                    Directory.CreateDirectory(filePath);
                }
                using (var fileStream = new FileStream(Path.Combine(filePath, fileName), FileMode.Create))
                {
                    fileStream.Write(filetoUpload.FileAsByteArray, 0,
                              filetoUpload.FileAsByteArray.Length);
                }
            }
        }
        public bool InsertViews(int NewsId, int MemberId)
        {
            try
            {
                SqlParameter[] prms = new SqlParameter[3];
                prms[0] = new SqlParameter("@id", NewsId);
                prms[1] = new SqlParameter("@MemberID", MemberId);
                prms[2] = new SqlParameter("@type", "InsertViews");
                (new DBHelpers(_configuration).ExecuteNonQuery)("sp_News", prms);
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public List<NewsUsers> GetNewsViewers(int NewsId)
        {
            try
            {
                List<NewsUsers> listObjNews = new List<NewsUsers>();
                SqlParameter[] prms = new SqlParameter[2];
                prms[0] = new SqlParameter("@type", "GetNewsViewers");
                prms[1] = new SqlParameter("@id", NewsId);
                DataTable dt = (new DBHelpers(_configuration).GetTableRow)("sp_News", prms);
                if (dt != null)
                {
                    if (dt.Rows.Count > 0)
                    {
                        for (int i = 0; i < dt.Rows.Count; i++)
                        {
                            NewsUsers Usersobj = new NewsUsers();
                            Usersobj.NewsID = Convert.ToInt32(dt.Rows[i]["NewsID"]);
                            Usersobj.MemberID = Convert.ToInt32(dt.Rows[i]["MemberID"]);
                            Usersobj.UserName = Convert.ToString(dt.Rows[i]["UserName"]);

                            listObjNews.Add(Usersobj);
                        }
                    }
                }
                return listObjNews;
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        public List<NewsUsers> GetLikesDislikesUsers(int NewsId, int LikeType)
        {
            try
            {
                List<NewsUsers> listObjNews = new List<NewsUsers>();
                SqlParameter[] prms = new SqlParameter[3];
                prms[0] = new SqlParameter("@type", "GetLikesDislikesUsers");
                prms[1] = new SqlParameter("@id", NewsId);
                prms[2] = new SqlParameter("@likeType", LikeType);
                DataTable dt = (new DBHelpers(_configuration).GetTableRow)("sp_News", prms);
                if (dt != null)
                {
                    if (dt.Rows.Count > 0)
                    {
                        for (int i = 0; i < dt.Rows.Count; i++)
                        {
                            NewsUsers Usersobj = new NewsUsers();
                            Usersobj.NewsID = Convert.ToInt32(dt.Rows[i]["NewsID"]);
                            Usersobj.MemberID = Convert.ToInt32(dt.Rows[i]["MemberID"]);
                            Usersobj.UserName = Convert.ToString(dt.Rows[i]["UserName"]);

                            listObjNews.Add(Usersobj);
                        }
                    }
                }
                return listObjNews;
            }
            catch (Exception ex)
            {
                return null;
            }
        }
    }
}
