using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace MTNews.Models
{
    public class CommentsResponse
    {
        public ObservableCollection<CommentsResponseData> data { get; set; }
    }
    
    public class CommentsResponseData : BaseViewModel
    {
        public int id { get; set; }
        public int userId { get; set; }

        private string _comment;
        public string comment
        {
            get { return _comment; }
            set { SetProperty(ref _comment, value); }
        }
        public bool isActive { get; set; }
        public int newsFeedId { get; set; }
        public DateTime createdDate { get; set; }
        public int createdBy { get; set; }
        public DateTime modifiedDate { get; set; }
        public int modifiedBy { get; set; }
        public string UserName { get; set; }

        private bool _isMenu = false;
        public bool IsMenuVisible 
        {
            get { return _isMenu; }
            set { SetProperty(ref _isMenu, value); }
        }
        private int isCount = 1;
        public int IsCount
        {
            get { return isCount; }
            set { SetProperty(ref isCount, value); }
        }
        private string profilePic;
        public string ProfilePic
        {
            get { return profilePic; }
            set { SetProperty(ref profilePic, value); }
        }

        private string shortName;
        public string ShortName
        {
            get { return shortName; }
            set { SetProperty(ref shortName, value); }
        }
    }
}

