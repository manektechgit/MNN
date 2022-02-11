using MNN.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MNN.WebAPI.Concrete
{
    public interface IUserService
    {
       Users AuthenticateUser(Login login);
       bool UpdateUser(UpdatedUser user);
    }
}
