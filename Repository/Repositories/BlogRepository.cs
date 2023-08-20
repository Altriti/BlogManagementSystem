using Domain.Model;
using Microsoft.EntityFrameworkCore;
using Repository.Core;
using Repository.DataContext;

namespace Repository.Repositories
{
    public interface IBlogRepository
    {
        IQueryable<Blog> GetAllBlogs(BlogParams blogParams);

        Blog GetBlogById(Guid id);

        void AddBlog(Blog blog);

        void UpdateBlog(Blog blog);

        void RemoveBlog(Blog blog);

        void SaveChanges();
    }


    public class BlogRepository : IBlogRepository
    {
        private readonly MyDataContext context;

        public BlogRepository(MyDataContext context)
        {
            this.context = context;
        }

        public IQueryable<Blog> GetAllBlogs(BlogParams blogParams)
        {
            var query = this.context.Blogs
                .Include(b => b.AppUser)
                .Include(b => b.Comments)
                .Where(b => b.PublicationDate >= blogParams.StartDate)
                .AsQueryable();

            return query;
        }

        public Blog GetBlogById(Guid id)
        {
            return this.context.Blogs.Include(b => b.AppUser).FirstOrDefault(b => b.Id == id);
        }

        public void AddBlog(Blog blog)
        {
            this.context.Blogs.Add(blog);
        }

        public void UpdateBlog(Blog blog)
        {
            var existingBlog = this.context.Blogs.Find(blog.Id);

            if (existingBlog != null)
            {
                existingBlog.Title = blog.Title;
                existingBlog.Content = blog.Content;
                existingBlog.Tags = blog.Tags;
            }
        }

        public void RemoveBlog(Blog blog)
        {
            this.context.Remove(blog);
        }

        public void SaveChanges()
        {

            var result = this.context.SaveChanges() > 0;

            if (!result)
            {
                throw new System.Exception($"Failed to complete task");
            }
        }
    }
}
