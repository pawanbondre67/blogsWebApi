using blogApi.Data;
using blogApi.Models;
using blogApi.Repositories;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

public class CommentRepository : Repository<Comment>, ICommentRepository
{
    public CommentRepository(BlogContext context) : base(context) { }

    public override async Task<Comment> AddAsync(Comment comment)
    {
        _context.Comments.Add(comment);
        await _context.SaveChangesAsync();

        // Load related data
        await _context.Entry(comment)
            .Reference(c => c.BlogPost)
            .LoadAsync();
        await _context.Entry(comment)
            .Reference(c => c.User)
            .LoadAsync();

        return comment;
    }

    public override async Task<IEnumerable<Comment>> FindAsync(Expression<Func<Comment, bool>> predicate)
    {
        return await _context.Comments
            .Include(c => c.User)
            .Where(predicate)
            .ToListAsync();
    }

}