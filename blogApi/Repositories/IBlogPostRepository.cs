using blogApi.Models;



namespace blogApi.Repositories
{
    public interface IBlogPostRepository : IRepository<BlogPost>
    {
        Task<IEnumerable<BlogPost>> GetByCategoryAsync(int categoryId);
        Task<IEnumerable<BlogPost>> GetPaginatedAsync(int page, int pageSize);
    }
}