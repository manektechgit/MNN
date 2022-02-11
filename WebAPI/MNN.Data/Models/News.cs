using System;
using System.Collections.Generic;
using System.Text;

namespace MNN.Data.Models
{
    public class News
    {
        public long Id { get; set; }
        public string Title { get; set; }
        public string Picture { get; set; }
        public string Content { get; set; }
        public bool IsActive { get; set; }
        public int CreatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public int ModifiedBy { get; set; }

        public DateTime? ModifiedDate { get; set; }

        public int? NumberOfLikes { get; set; }

        public int? NumberOfDisLikes { get; set; }
        public int? NumberOfComments { get; set; }
        public int? NumberOfViews { get; set; }
        public FileToUpload Image { get; set; }
    }

    public class FileToUpload
    {
        public string fileName { get; set; }
        public string fileSize { get; set; }
        public string fileType { get; set; }
        public string fileAsBase64 { get; set; }
        public byte[] FileAsByteArray { get; set; }
    }
}
