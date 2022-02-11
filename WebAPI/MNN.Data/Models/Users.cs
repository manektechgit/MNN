using System;
using System.Collections.Generic;
using System.Text;

namespace MNN.Data.Models
{
    public class Users
    {
        public long UserId { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string Gender { get; set; }
        public string Designation { get; set; }
        public string About { get; set; }
        public DateTime BirthDate { get; set; }
        public byte[] ProfilePicture { get; set; }
        public string Firstname { get; set; }
        public string Lastname { get; set; }
        public DateTime JoiningDate { get; set; }
        public bool IsActive { get; set; }

    }
    public class UpdatedUser
    {
        public int MemberID { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int RoleID { get; set; }
        public int DepartmentID { get; set; }
        public string PersonalEmail { get; set; }
        public int Experience { get; set; }
        public string Gender { get; set; }
        public int ReportingTo { get; set; }
        public int ShiftID { get; set; }
        public bool IsActive { get; set; }
        public string UserName { get; set; }
        public string MobileNo { get; set; }
        public string PhoneNo { get; set; }
        public string Address { get; set; }
        public DateTime BirthDate { get; set; }
        public DateTime CreatedDate { get; set; }
    }

    public class NewsUsers
    {
        public int NewsID { get; set; }
        public int MemberID { get; set; }
        public string UserName { get; set; }
    }
}
