using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;

namespace MTNews.Models
{
    public class LoginResponse
    {
        public string StatusCode { get; set; }
        public string Message { get; set; }
        public LoginResponseData Data { get; set; }
    }

    public class LoginResponseData
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

    public class LoginString
    {
        public string value { get; set; }
        public string xmlns { get; set; }
    }
     
    public class LoginStringRoot
    {
        public LoginString @string { get; set; }
    }
}
