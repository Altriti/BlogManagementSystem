using Domain.Model;
using Repository.Repositories;
using Service.serviceInterfaces;

namespace Service.services
{
    public class CommentService : ICommentService
    {
        private readonly ICommentRepository commentRepository;
        private readonly IBlogRepository blogRepository;
        private readonly UserAccessor userAccessor;
        private readonly IAccountRepository accountRepository;

        public CommentService(ICommentRepository commentRepository, IBlogRepository blogRepository,
            UserAccessor userAccessor, IAccountRepository accountRepository)
        {
            this.commentRepository = commentRepository;
            this.blogRepository = blogRepository;
            this.userAccessor = userAccessor;
            this.accountRepository = accountRepository;
        }

        public List<Comment> GetComments()
        {
            return this.commentRepository.GetCommentsFromDb();
        }

        public string AddComment(Comment comment, Guid blogId)
        {
            try
            {
                var populatedComment = PopulateCommentFields(comment);

                var blog = this.blogRepository.GetBlogById(blogId);

                blog.Comments.Add(populatedComment);

                this.commentRepository.AddCommentToDb(populatedComment);

                this.commentRepository.SaveChanges();

                return "Comment added successfully";
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        private Comment PopulateCommentFields(Comment comment)
        {
            var userId = this.userAccessor.GetUserId();

            if (userId != null)
            {
                var user = this.accountRepository.FindUserById(userId);

                return new Comment
                {
                    Id = comment.Id,
                    AuthorName = user.DisplayName,
                    AuthorEmail = user.Email,
                    Content = comment.Content,
                };
            }

            return new Comment
            {
                Id = comment.Id,
                AuthorName = comment.AuthorName,
                AuthorEmail = comment.AuthorEmail,
                Content = comment.Content,
            };
        }
    }
}
