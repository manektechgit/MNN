using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using MNN.Data.Models;
using MNN.WebAPI.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MNN.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private IUserService _userService;
        private readonly IConfiguration _configuration;
        public UsersController(IUserService userService, IConfiguration configuration)
        {
            _userService = userService;
            _configuration = configuration;
        }


        [HttpPost("authenticate")]
        public IActionResult Authenticate(Login login)
        {
            var response = _userService.AuthenticateUser(login);

            if (response == null)
                return BadRequest(new { message = "Username or password is incorrect" });

            return Ok(response);
        }
        [HttpPost("updateUser")]
        public IActionResult UpdateUser(UpdatedUser objuser)
        {
            var response = _userService.UpdateUser(objuser);
            if (!response)
                return BadRequest(new { message = "Something went wrong!!" });
            return Ok(response);
        }
    }
}
