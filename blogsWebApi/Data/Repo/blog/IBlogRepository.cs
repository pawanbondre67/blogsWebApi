using blogsWebApi.Models;

namespace blogsWebApi.Data.Repo.blog
{
    public interface IBlogRepository
    {

        Task<Blog> GetBlogByIdAsync(int id);
        Task<IEnumerable<Blog>> GetAllBlogsAsync();
        Task<IEnumerable<Blog>> GetBlogsByCategoryAsync(int categoryId);
        Task<IEnumerable<Blog>> GetBlogsByAuthorAsync(int authorId);
        Task<IEnumerable<Blog>> SearchBlogsAsync(string searchTerm);
      Task<Blog> AddBlogAsync(Blog blog);
        Task UpdateBlogAsync(Blog blog);
        Task DeleteBlogAsync(int id);
        Task LikeBlogAsync(int blogId, int userId);
        Task UnlikeBlogAsync(int blogId, int userId);
    }
}
