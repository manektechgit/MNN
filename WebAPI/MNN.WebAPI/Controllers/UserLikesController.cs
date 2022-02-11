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
    public class UserLikesController : ControllerBase
    {
        private IUserLikesService _userLikesService;
        private readonly IConfiguration _configuration;

        public UserLikesController(IConfiguration configuration, IUserLikesService userLikesService)
        {
            _userLikesService = userLikesService;
            _configuration = configuration;
        }

        [HttpPost("createLikes")]
        public IActionResult CreateLikes(UserLikes objUserlikes)
        {
            var response = _userLikesService.AddLikes(objUserlikes);
            if (!response)
                return BadRequest(new { message = "Something went wrong!!" });
            return Ok(response);
        }

        [HttpPost("updateLikes")]
        public IActionResult UpdateLikes(UserLikes objUserlikes)
        {
            var response = _userLikesService.UpdateLikes(objUserlikes);
            if (!response)
                return BadRequest(new { message = "Something went wrong!!" });
            return Ok(response);
        }

        [HttpPost("getLikes")]
        public IActionResult GerLikes(GetUserLikes objUserlikes)
        {
            var response = _userLikesService.GetLikes(objUserlikes);
            if (response == null)
                return BadRequest(new { message = "Something went wrong!!" });
            return Ok(response);
        }
    }
}
