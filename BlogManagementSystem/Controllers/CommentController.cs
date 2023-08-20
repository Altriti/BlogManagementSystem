using Domain.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Service.serviceInterfaces;

namespace BlogManagementSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CommentController : ControllerBase
    {
        private readonly ICommentService commentService;

        public CommentController(ICommentService commentService)
        {
            this.commentService = commentService;
        }

        [HttpGet]
        [Route("getall")]
        public ActionResult<Comment> GetComments()
        {
            return Ok(this.commentService.GetComments());
        }

        [HttpPost("add")]
        public IActionResult CreateBlog(Comment comment, Guid blogId)
        {
            return Ok(this.commentService.AddComment(comment, blogId));
        }
    }
}
