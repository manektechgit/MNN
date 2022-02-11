using System;
using System.Collections.Generic;
using System.Text;

namespace MNN.Data.Models
{
    public class UserLikes
    {
        public long Id { get; set; }
        public long UserId { get; set; }
        public int IsLiked { get; set; }
        public long NewsFeedId { get; set; }
        public DateTime? LikedDate { get; set; }
       
    }
    public class GetUserLikes
    {
        public long UserId { get; set; }
        public long NewsFeedId { get; set; }
    }
}
