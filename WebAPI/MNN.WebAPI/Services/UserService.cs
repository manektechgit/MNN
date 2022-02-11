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
    public class UserService: IUserService
    {
        private readonly IConfiguration _configuration;
        public UserService(IConfiguration configuration)
        {
            _configuration = configuration;

        }
        public Users AuthenticateUser(Login login)
        {
            Users user = new Users();
            SqlParameter[] prms = new SqlParameter[2];
            prms[0] = new SqlParameter("@Email", login.Email);
            prms[1] = new SqlParameter("@Password", login.Password);
            DataTable dt = (new DBHelpers(_configuration).GetTableRow)("sp_AuthenticateUser", prms);
            if (dt != null)
            {
                if (dt.Rows.Count > 0)
                {
                    user.Firstname = Convert.ToString(dt.Rows[0]["Firstname"]);
                    user.Lastname = Convert.ToString(dt.Rows[0]["Lastname"]);
                    user.Gender = Convert.ToString(dt.Rows[0]["Gender"]);
                    if(!string.IsNullOrEmpty(dt.Rows[0]["BirthDate"].ToString()))
                    {
                        user.BirthDate = Convert.ToDateTime(dt.Rows[0]["BirthDate"]);
                    }
                   
                    user.Designation = Convert.ToString(dt.Rows[0]["Designation"]);
                    user.About = Convert.ToString(dt.Rows[0]["About"]);
                    //user.ProfilePicture = Convert.ToString(dt.Rows[0]["ProfilePicture"]);

                }
            }
            return user;
        }

        public bool UpdateUser(UpdatedUser user)
        {
            try
            {
                SqlParameter[] prms = new SqlParameter[18];

                prms[0] = new SqlParameter("@MemberID", user.MemberID);
                prms[1] = new SqlParameter("@FirstName", user.FirstName);
                prms[2] = new SqlParameter("@LastName", user.LastName);
                prms[3] = new SqlParameter("@RoleID", user.RoleID);
                prms[4] = new SqlParameter("@DepartmentID", user.DepartmentID);
                prms[5] = new SqlParameter("@PersonalEmail", user.PersonalEmail);
                prms[6] = new SqlParameter("@Experience", user.Experience);
                prms[7] = new SqlParameter("@Gender", user.Gender);
                prms[8] = new SqlParameter("@ReportingTo", user.ReportingTo);
                prms[9] = new SqlParameter("@ShiftID", user.ShiftID);
                prms[10] = new SqlParameter("@IsActive", user.IsActive);
                prms[11] = new SqlParameter("@UserName", user.UserName);
                prms[12] = new SqlParameter("@MobileNo", user.MobileNo);
                prms[13] = new SqlParameter("@PhoneNo", user.PhoneNo);
                prms[14] = new SqlParameter("@Address", user.Address);
                prms[15] = new SqlParameter("@BirthDate", user.BirthDate);
                prms[16] = new SqlParameter("@CreatedDate", user.CreatedDate);
                prms[17] = new SqlParameter("@type", "UpdateUser");

                (new DBHelpers(_configuration).ExecuteNonQuery)("sp_Users", prms);
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
    }
}
