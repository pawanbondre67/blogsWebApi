using blogApi.Data;
using blogApi.Models;

using Microsoft.EntityFrameworkCore;


namespace blogApi.Repositories
{
    public class CategoryRepository : Repository<Category>, ICategoryRepository
    {
        public CategoryRepository(BlogContext context) : base(context) { }

        public async Task<IEnumerable<Category>> GetAllWithPostCountAsync()
            => await _context.Categories
                .Include(c => c.BlogPosts)
                .ToListAsync();

        public async Task<bool> CategoryExistsAsync(string name)
            => await _context.Categories.AnyAsync(c => c.Name.ToLower() == name.ToLower());

        public async Task<Category> GetByIdAsync(int id)
        {
            return await _context.Categories
                .Include(c => c.BlogPosts)
                .FirstOrDefaultAsync(c => c.Id == id);
        }
    }
}