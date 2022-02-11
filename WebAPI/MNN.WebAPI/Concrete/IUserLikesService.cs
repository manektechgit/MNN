using MNN.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MNN.WebAPI.Concrete
{
  public interface IUserLikesService
    {
        bool AddLikes(UserLikes objUserLikes);
        bool UpdateLikes(UserLikes objUserLikes);
        UserLikes GetLikes(GetUserLikes objUserLikes);
    }
}
