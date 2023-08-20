
using Domain.Model;
using Microsoft.AspNetCore.Identity;
using Repository.Core;
using Repository.Repositories;
using Service.Contract;

namespace Service.Implementation
{
    public class BlogService : IBlogService
    {
        private readonly IBlogRepository blogRepository;
        private readonly UserAccessor userAccessor;
        private readonly IAccountRepository accountRepository;
        private readonly RoleManager<IdentityRole> roleManager;

        public BlogService(IBlogRepository blogRepository, UserAccessor userAccessor,
            IAccountRepository accountRepository, RoleManager<IdentityRole> roleManager)
        {
            this.blogRepository = blogRepository;
            this.userAccessor = userAccessor;
            this.accountRepository = accountRepository;
            this.roleManager = roleManager;
        }

        public async Task<PagedList<Blog>> GetAllBlogs(BlogParams blogParams)
        {
            var query = this.blogRepository.GetAllBlogs(blogParams);

            var result = await PagedList<Blog>
                .CreateAsync(query, blogParams.PageNumber, blogParams.PageSize);

            return result;
        }

        public Blog GetBlogById(Guid id)
        {
            var blog = this.blogRepository.GetBlogById(id);

            if (blog != null)
            {
                return blog;
            }

            throw new System.Exception($"Blog with ID {id} could not be found");
        }

        public string AddBlog(Blog blog)
        {
            try
            {
                var loggedinUserId = this.LoggedinUserId();

                blog.AppUserId = loggedinUserId;

                blog.PublicationDate = DateTime.Now;

                this.blogRepository.AddBlog(blog);

                this.blogRepository.SaveChanges();

                return "Blog added successfully";
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        public string UpdateBlog(Blog blog)
        {
            try
            {
                this.blogRepository.UpdateBlog(blog);

                this.blogRepository.SaveChanges();

                return "Blog updated successfully";
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        public string RemoveBlog(Guid id)
        {
            try
            {
                var loggedinUserId = this.LoggedinUserId();

                var roleUser = this.accountRepository.RoleUser(loggedinUserId);

                var role = this.roleManager.Roles.FirstOrDefault(r => r.Id == roleUser.RoleId);

                var blogToDelete = this.blogRepository.GetBlogById(id);

                if (blogToDelete.AppUserId == loggedinUserId || role.Name == "Administrator")
                {
                    var existingBlog = this.blogRepository.GetBlogById(id);

                    if (existingBlog != null)
                    {
                        this.blogRepository.RemoveBlog(existingBlog);

                        this.blogRepository.SaveChanges();

                        return "Blog deleted successfully ";
                    }

                    return $"Could not find blog";
                }

                return "You are not blog creator or you are not administrator";
            }

            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        public string LoggedinUserId()
        {

            var loggedinUserId = this.userAccessor.GetUserId();

            if (loggedinUserId != null)
            {
                return loggedinUserId;
            }

            return "You should be logged in";
        }
    }
}
