using Microsoft.AspNetCore.Cors;
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
    
    public class NewsController : ControllerBase
    {
        private INewsService _newsService;
        private readonly IConfiguration _configuration;

        public NewsController(IConfiguration configuration, INewsService newsService)
        {
            _newsService = newsService;
            _configuration = configuration;
        }

        [HttpPost("createNews")]
        public IActionResult CreateNews(News newsObj)
        {
            var response = _newsService.AddNews(newsObj);
            if (!response)
                return BadRequest(new { message = "Something went wrong!!" });
            return Ok(response);
        }
        [HttpPost("updateNews")]
        public IActionResult UpdateNews(News newsObj)
        {
            var response = _newsService.UpdateNews(newsObj);
            if (!response)
                return BadRequest(new { message = "Something went wrong!!" });
            return Ok(response);
        }

        [HttpPost("deleteNews")]
        public IActionResult DeleteNews(int id)
        {
            var response = _newsService.DeleteNews(id);
            if (!response)
                return BadRequest(new { message = "Something went wrong!!" });
            return Ok(response);
        }


        [HttpGet("GetNewsById")]
        public IActionResult GetNewsById(int id)
        {
            var response = _newsService.GetNewsById(id);
            if (response == null)
                return BadRequest(new { message = "Something went wrong!!" });
            return Ok(response);
        }
        [HttpGet("GetAllNews")]
        public IActionResult GetAllNews(int pagesize =10, int pagenumber = 1, string? searchText = null)
        {
            var response = _newsService.GetAllNews(pagesize, pagenumber, searchText);
            if (response == null)
                return BadRequest(new { message = "Something went wrong!!" });
            return Ok(response);
        }
        [HttpGet("GetAllNewsAdmin")]
        public IActionResult GetAllNewsAdmin(int pagesize = 10, int pagenumber = 1, string? searchText = null)
        {
            var response = _newsService.GetAllNewsAdmin(pagesize, pagenumber, searchText);
            if (response == null)
                return BadRequest(new { message = "Something went wrong!!" });
            return Ok(response);
        }
        [HttpPost("UploadFile"), RequestSizeLimit(100_000_000)]
        public IActionResult UploadFile([FromBody] IFormFile file)
        {
            var response = _newsService.UploadFile(file);
            if (response == null)
                return BadRequest(new { message = "Something went wrong!!" });
            return Ok(response);
        }
        [HttpPost("InsertViews")]
        public IActionResult InsertViews(int NewsId, int MemberId)
        {
            var response = _newsService.InsertViews(NewsId, MemberId);
            if (!response)
                return BadRequest(new { message = "Something went wrong!!" });
            return Ok(response);
        }
        [HttpGet("GetNewsViewers")]
        public IActionResult GetNewsViewers(int NewsId)
        {
            var response = _newsService.GetNewsViewers(NewsId);
            if (response == null)
                return BadRequest(new { message = "Something went wrong!!" });
            return Ok(response);
        }

        [HttpPost("ChangeNewsStatus")]
        public IActionResult ChangeNewsStatus(int NewsId, bool status)
        {
            var response = _newsService.Changestatus(status,NewsId);
            if (response == false)
                return BadRequest(new { message = "Something went wrong!!" });
            return Ok(response);
        }
        [HttpGet("GetLikesDislikesUsers")]
        public IActionResult GetLikesDislikesUsers(int NewsId, int LikeType)
        {
            var response = _newsService.GetLikesDislikesUsers(NewsId, LikeType);
            if (response == null)
                return BadRequest(new { message = "Something went wrong!!" });
            return Ok(response);
        }
    }
}
