using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace MTNews.Models
{
    public class NewsResponse
    {
        public ObservableCollection<NewsResponseData> data { get; set; }
    }
    
    public class NewsResponseData : BaseViewModel
    {
        public int id { get; set; }
        public string title { get; set; }
        public string picture { get; set; }
        public string content { get; set; }
        public bool isActive { get; set; }
        public int createdBy { get; set; }
        public DateTime createdDate { get; set; }
        public int modifiedBy { get; set; }
        public object modifiedDate { get; set; }
        public int numberOfLikes { get; set; }
        public int numberOfComments { get; set; }
        public string image { get; set; }
        public int numberOfDisLikes { get; set; }
        public int numberOfViews { get; set; }

        private string newsPic;
        public string NewsPic
        {
            get { return newsPic; }
            set { SetProperty(ref newsPic, value); }
        }
    }

    public class NewsViewers : BaseViewModel
    {
        public int newsID { get; set; }
        public int memberID { get; set; } 
        public string userName { get; set; }

        private string shortName;
        public string ShortName
        {
            get { return shortName; }
            set { SetProperty(ref shortName, value); }
        }
    }
}
