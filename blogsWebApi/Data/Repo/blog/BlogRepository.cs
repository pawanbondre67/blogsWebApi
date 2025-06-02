using blogsWebApi.Models;
using Microsoft.EntityFrameworkCore;

namespace blogsWebApi.Data.Repo.blog
{
    public class BlogRepository : IBlogRepository
    {
        private readonly DataContext db;

        public BlogRepository(DataContext db)
        {
            this.db = db;
            // Initialize any required resources here, such as a database context
        }
        public async Task<Blog> AddBlogAsync(Blog blog)
        {
            if (blog == null)
                throw new ArgumentNullException(nameof(blog), "Blog cannot be null");

            await db.Blogs.AddAsync(blog);
            await db.SaveChangesAsync();

            return blog;
        }

        public async Task<IEnumerable<Blog>> GetAllBlogsAsync()
        {
            return await db.Blogs.ToListAsync();
        }

        public async Task<Blog> GetBlogByIdAsync(int id)
        {
            return await db.Blogs.Include(b => b.Author).Include(b => b.Category)
                                 .FirstOrDefaultAsync(b => b.Id == id);
        }

        public async Task UpdateBlogAsync(Blog blog)
        {
            db.Blogs.Update(blog);
            await db.SaveChangesAsync();
        }

        public async Task DeleteBlogAsync(int id)
        {
            var blog = await db.Blogs.FindAsync(id);
            if (blog != null)
            {
                db.Blogs.Remove(blog);
                await db.SaveChangesAsync();
            }
        }

        public async Task<IEnumerable<Blog>> GetBlogsByAuthorAsync(int authorId)
        {
            return await db.Blogs.Where(b => b.AuthorId == authorId).ToListAsync();
        }

        public async Task<IEnumerable<Blog>> GetBlogsByCategoryAsync(int categoryId)
        {
            return await db.Blogs.Where(b => b.CategoryId == categoryId).ToListAsync();
        }

        public async Task<IEnumerable<Blog>> SearchBlogsAsync(string searchTerm)
        {
            return await db.Blogs
                .Where(b => b.Title.Contains(searchTerm) || b.Content.Contains(searchTerm))
                .ToListAsync();
        }

        public Task LikeBlogAsync(int blogId, int userId)
        {
            throw new NotImplementedException();
        }

 

        public Task UnlikeBlogAsync(int blogId, int userId)
        {
            throw new NotImplementedException();
        }

      
    }
}
