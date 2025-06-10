using blogApi.Models;
using blogApi.Repositories;

namespace blogApi.Repositories
{
    public interface ICategoryRepository : IRepository<Category>
    {
        Task<IEnumerable<Category>> GetAllWithPostCountAsync();
        Task<bool> CategoryExistsAsync(string name);
    }
}