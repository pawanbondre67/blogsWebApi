using blogApi.Data;
using blogApi.Models;

using Microsoft.EntityFrameworkCore;

namespace blogApi.Repositories
{
    public class BlogPostRepository : Repository<BlogPost>, IBlogPostRepository
    {
        public BlogPostRepository(BlogContext context) : base(context) { }

        public async Task<IEnumerable<BlogPost>> GetByCategoryAsync(int categoryId)
            => await _context.BlogPosts.Where(b => b.CategoryId == categoryId).ToListAsync();

        public async Task<IEnumerable<BlogPost>> GetPaginatedAsync(int page, int pageSize)
            => await _context.BlogPosts
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
    }
}