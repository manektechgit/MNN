using System;
using System.Collections.Generic;
using System.Text;

namespace MTNews.Models
{
    public class PaginationRequest
    {
        public int Id { get; set; }
        public int PageSize { get; set; }
        public int PageNumber { get; set; }
        public string SearchText { get; set; }
    }
}
