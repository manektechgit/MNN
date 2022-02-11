using System;
using System.Collections.Generic;
using System.Text;

namespace MNN.Data.Models
{
    public class Comments
    {
        public long Id { get; set; }
        public long UserId { get; set; }
        public string Comment { get; set; }
        public bool IsActive { get; set; }
        public long NewsFeedId { get; set; }
        public DateTime? CreatedDate { get; set; }
        public int CreatedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public int ModifiedBy { get; set; }
        public string UserName { get; set; }
    }
}
