using Domain.Model;
using Repository.DataContext;

namespace Repository.Repositories
{
    public interface ICommentRepository
    {
        List<Comment> GetCommentsFromDb();
        void AddCommentToDb(Comment comment);
        void SaveChanges();
    }


    public class CommentRepository : ICommentRepository
    {
        private readonly MyDataContext context;

        public CommentRepository(MyDataContext context)
        {
            this.context = context;
        }

        public List<Comment> GetCommentsFromDb()
        {
            return this.context.Comments.ToList();
        }

        public void AddCommentToDb(Comment comment)
        {
            this.context.Add(comment);
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
