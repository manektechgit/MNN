using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using MNN.Data.Models;
using MNN.WebAPI.Concrete;
using MNN.WebAPI.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MNN.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CommentsController : ControllerBase
    {
        private ICommnetsService _commentsService;
        private readonly IConfiguration _configuration;

        public CommentsController(IConfiguration configuration, ICommnetsService commentsService)
        {
            _commentsService = commentsService;
            _configuration = configuration;
        }
        [HttpPost("createComment")]
        public IActionResult CreateComment(Comments objComments)
        {
            var response = _commentsService.AddComment(objComments);
            if (!response)
                return BadRequest(new { message = "Something went wrong!!" });
            return Ok(response);
        }

        [HttpPost("updateComment")]
        public IActionResult UpdateComment(Comments objComments)
        {
            var response = _commentsService.UpdateComment(objComments);
            if (!response)
                return BadRequest(new { message = "Something went wrong!!" });
            return Ok(response);
        }
        [HttpPost("deleteComment")]
        public IActionResult DeleteComment(int id)
        {
            var response = _commentsService.DeleteComment(id);
            if (!response)
                return BadRequest(new { message = "Something went wrong!!" });
            return Ok(response);
        }
        [HttpGet("GetCommentById")]
        public IActionResult GetCommentById(int id, int pagenumber, int pagesize)
        {
            var response = _commentsService.GetCommentsById(id, pagenumber, pagesize);
            if (response == null)
                return BadRequest(new { message = "Something went wrong!!" });
            return Ok(response);
        }
    }
}
