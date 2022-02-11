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
    public class CommnetsService : ICommnetsService
    {
        private readonly IConfiguration _configuration;
        public CommnetsService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public bool AddComment(Comments objComments)
        {
            try
            {
                SqlParameter[] prms = new SqlParameter[9];
             
                prms[0] = new SqlParameter("@userId", objComments.UserId);

                 
                prms[1] = new SqlParameter("@comment", (objComments.Comment));
                 
              

                prms[2] = new SqlParameter("@isActive", objComments.IsActive);
                prms[3] = new SqlParameter("@newsFeedId", objComments.NewsFeedId);
                prms[4] = new SqlParameter("@createdDate", objComments.CreatedDate);
                prms[5] = new SqlParameter("@createdBy", objComments.CreatedBy);
                prms[6] = new SqlParameter("@modifiedDate", objComments.ModifiedDate);
                prms[7] = new SqlParameter("@modifiedBy", objComments.ModifiedBy);
                prms[8] = new SqlParameter("@type", "Insert");

                (new DBHelpers(_configuration).ExecuteNonQuery)("sp_Comment", prms);
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }


        public bool UpdateComment(Comments objComments)
        {
            try
            {
                SqlParameter[] prms = new SqlParameter[10];
                prms[0] = new SqlParameter("@id", objComments.Id);
                prms[1] = new SqlParameter("@userId", objComments.UserId);

                prms[2] = new SqlParameter("@comment", (objComments.Comment));

                prms[3] = new SqlParameter("@isActive", objComments.IsActive);
                prms[4] = new SqlParameter("@newsFeedId", objComments.NewsFeedId);
                prms[5] = new SqlParameter("@createdDate", objComments.CreatedDate);
                prms[6] = new SqlParameter("@createdBy", objComments.CreatedBy);
                prms[7] = new SqlParameter("@modifiedDate", objComments.ModifiedDate);
                prms[8] = new SqlParameter("@modifiedBy", objComments.ModifiedBy);
                
                prms[9] = new SqlParameter("@type", "Update");
                
                (new DBHelpers(_configuration).ExecuteNonQuery)("sp_Comment", prms);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public bool DeleteComment(int id)
        {
            try
            {
                SqlParameter[] prms = new SqlParameter[2];
                prms[0] = new SqlParameter("@id", id);
                prms[1] = new SqlParameter("@type", "Delete");
                (new DBHelpers(_configuration).ExecuteNonQuery)("sp_Comment", prms);
                return true;
            }
            catch(Exception ex)
            {
                return false;
            }
        }

        public List<Comments> GetCommentsById(int id, int pagenumber, int pagesize)
        {
            List<Comments> listObjNews = new List<Comments>();
            SqlParameter[] prms = new SqlParameter[4];
            prms[0] = new SqlParameter("@type", "GetCommentsById");
            prms[1] = new SqlParameter("@id", id);
            prms[2] = new SqlParameter("@pagenumber", pagenumber);
            prms[3] = new SqlParameter("@pagesize", pagesize);
            DataTable dt = (new DBHelpers(_configuration).GetTableRow)("sp_Comment", prms);
            if (dt != null)
            {
                if (dt.Rows.Count > 0)
                {
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        Comments Commentsobj = new Comments();
                        Commentsobj.Id = Convert.ToInt32(dt.Rows[i]["Id"]);
                        Commentsobj.UserId = Convert.ToInt32(dt.Rows[i]["UserId"]);
                        Commentsobj.Comment = Convert.ToString(dt.Rows[i]["Comment"]);
                        Commentsobj.CreatedDate = Convert.ToDateTime(dt.Rows[i]["CreatedDate"]);
                        Commentsobj.CreatedBy = Convert.ToInt32(dt.Rows[i]["CreatedBy"]);
                        Commentsobj.ModifiedBy = Convert.ToInt32(dt.Rows[i]["ModifiedBy"]);
                        Commentsobj.ModifiedDate = Convert.ToDateTime(dt.Rows[i]["ModifiedDate"]);
                        Commentsobj.UserName = Convert.ToString(dt.Rows[i]["UserName"]);

                        listObjNews.Add(Commentsobj);
                    }
                }
            }
            return listObjNews;
        }

    }
}
