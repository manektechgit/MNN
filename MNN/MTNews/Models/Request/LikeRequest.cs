using System;
using System.Collections.Generic;
using System.Text;

namespace MTNews.Models
{
    public class LikeRequest
    {
        public int id { get; set; }
        public int userId { get; set; }
        public int isLiked { get; set; }
        public int newsFeedId { get; set; }
        public DateTime? likedDate { get; set; }
    }

    public class LikeStatus
    {
        public int userId { get; set; }
        public int newsFeedId { get; set; }
    }
}
