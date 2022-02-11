using System;
using System.Collections.Generic;
using System.Text;

namespace MTNews.Models
{
    public class CreateCommentRequest
    {
        public int id { get; set; }
        public int userId { get; set; }
        public string comment { get; set; }
        public bool isActive { get; set; }
        public int newsFeedId { get; set; }
        public DateTime createdDate { get; set; }
        public int createdBy { get; set; }
        public DateTime modifiedDate { get; set; }
        public int modifiedBy { get; set; }
    }
}
