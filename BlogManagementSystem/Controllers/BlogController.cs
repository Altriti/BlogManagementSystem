using Domain.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Repository.Core;
using Service.Contract;

namespace BlogManagementSystem.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class BlogController : ControllerBase
    {
        private readonly IBlogService blog;

        public BlogController(IBlogService blog)
        {
            this.blog = blog;
        }

        [HttpGet]
        [Route("getall")]
        public async Task<ActionResult> GetBlogs([FromQuery] BlogParams param)
        {
            return Ok(await this.blog.GetAllBlogs(param));
        }

        [HttpGet("get")]
        public IActionResult GetBlog(Guid id)
        {
            return Ok(this.blog.GetBlogById(id));
        }

        [HttpPost("add")]
        public IActionResult CreateBlog(Blog blog)
        {
            return Ok(this.blog.AddBlog(blog));
        }

        [HttpPut("edit")]
        public IActionResult EditBlog(Blog blog)
        {
            return Ok(this.blog.UpdateBlog(blog));
        }

        [HttpDelete]
        public IActionResult DeleteBlog(Guid id)
        {
            return Ok(this.blog.RemoveBlog(id));
        }
    }
}
