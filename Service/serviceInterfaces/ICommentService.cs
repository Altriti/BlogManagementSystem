using Domain.Model;

namespace Service.serviceInterfaces
{
    public interface ICommentService
    {
        List<Comment> GetComments();
        string AddComment(Comment comment, Guid blogId);
    }
}
