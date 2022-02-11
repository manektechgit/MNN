using MNN.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MNN.WebAPI.Concrete
{
    public interface ICommnetsService
    {
        bool AddComment(Comments objComments);
        bool UpdateComment(Comments objComments);
        bool DeleteComment(int id);
        List<Comments> GetCommentsById(int id, int pagenumber, int pagesize);
    }
}
