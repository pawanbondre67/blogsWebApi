using blogApi.Data;
using blogApi.Models;

using Microsoft.EntityFrameworkCore;

namespace blogApi.Repositories
{
    public class BlogPostRepository : Repository<BlogPost>, IBlogPostRepository
    {
        public BlogPostRepository(BlogContext context) : base(context) { }

        public async Task<IEnumerable<BlogPost>> GetPaginatedAsync(int page, int pageSize)
        {
            return await _context.BlogPosts
                .Include(b => b.Category)
                .Include(b => b.Author)
                .Include(b => b.Comments)
                .Include(b => b.Likes)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
        }

        public async Task<IEnumerable<BlogPost>> GetByCategoryPaginatedAsync(int categoryId, int page, int pageSize)
        {
            return await _context.BlogPosts
                .Include(b => b.Category)
                .Include(b => b.Author)
                .Include(b => b.Comments)
                .Include(b => b.Likes)
                .Where(b => b.CategoryId == categoryId)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
        }

        public override async Task<BlogPost> AddAsync(BlogPost post)
        {
            // Use the protected _context property to access the DbSet
            _context.BlogPosts.Add(post);
            await _context.SaveChangesAsync();

            // Load related data for mapping
            await _context.Entry(post)
                .Reference(b => b.Category)
                .LoadAsync();
            await _context.Entry(post)
                .Reference(b => b.Author)
                .LoadAsync();
            // Comments and Likes are empty for new posts, so no need to load

            return post;
        }

        public override async Task UpdateAsync(BlogPost post)
        {
            // Use the protected _context property to access the DbSet
            _context.BlogPosts.Update(post);
            await _context.SaveChangesAsync();
            // Load related data for mapping
            await _context.Entry(post)
                .Reference(b => b.Category)
                .LoadAsync();
            await _context.Entry(post)
                .Reference(b => b.Author)
                .LoadAsync();
            // Comments and Likes are not updated here, so no need to load
        }

        public override async Task DeleteAsync(int id)
        {
            var post = await GetByIdAsync(id);
            if (post != null)
            {
                _context.BlogPosts.Remove(post);
                await _context.SaveChangesAsync();
            }
        }

        public override async Task<BlogPost> GetByIdAsync(int id)
        {
            var post = await _context.BlogPosts
                .Include(b => b.Category)
                .Include(b => b.Author)
                .Include(b => b.Comments)
                .Include(b => b.Likes)
                .FirstOrDefaultAsync(b => b.Id == id);
            //Console.WriteLine("Fetching post with ID: " + id ,post);
            return post;

        }

    }
}