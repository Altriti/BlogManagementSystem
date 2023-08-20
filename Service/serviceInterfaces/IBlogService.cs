
using Domain.Model;
using Repository.Core;

namespace Service.Contract
{
    public interface IBlogService
    {
        Task<PagedList<Blog>> GetAllBlogs(BlogParams blogParams);

        Blog GetBlogById(Guid id);

        string AddBlog(Blog blog);

        string UpdateBlog(Blog blog);

        string RemoveBlog(Guid id);
    }
}
