using blogApi.Models;

namespace blogApi.Repositories
{
    public interface ICommentRepository : IRepository<Comment> 
    {
        Task<Comment> AddAsync(Comment comment);

    }
}
