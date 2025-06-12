using blogApi.Models;



namespace blogApi.Repositories
{
    public interface IBlogPostRepository : IRepository<BlogPost>
    {
      
        Task<IEnumerable<BlogPost>> GetPaginatedAsync(int page, int pageSize);
        Task<BlogPost> AddAsync(BlogPost post); // Override base AddAsync

        Task UpdateAsync(BlogPost post); // Override base UpdateAsync

        Task DeleteAsync(int id); // Override base DeleteAsync

        Task<BlogPost> GetByIdAsync(int id); // Override base GetByIdAsync

        Task<IEnumerable<BlogPost>> GetByCategoryPaginatedAsync(int categoryId, int page, int pageSize);
    }
} 