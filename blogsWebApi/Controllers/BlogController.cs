using blogsWebApi.Data;
using blogsWebApi.Data.Repo.blog;
using blogsWebApi.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

//using blogsWebApi.Models;

namespace blogsWebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BlogController : ControllerBase
    {
        private readonly IBlogRepository repo;

        public BlogController(IBlogRepository  repo )
        {
            this.repo = repo;
        }

        [HttpGet("")]
        public async Task<IActionResult> GetAllBlogs()
        {
            var blogs = await repo.GetAllBlogsAsync();
            if (blogs == null || !blogs.Any())
            {
                return NotFound("No blogs found.");
            }
            return Ok(blogs);
        }

        // Create Blog

        [HttpPost]
        public async Task<IActionResult> CreateBlog([FromBody] Blog blog)
        {
            if (blog == null)
            {
                return BadRequest("Blog data is null.");
            }
            await repo.AddBlogAsync(blog);
            return CreatedAtAction(nameof(GetAllBlogs), new { id = blog.Id }, blog);
        }



        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteBlog(int id)
        {
            var existingBlog = await repo.GetBlogByIdAsync(id);
            if (existingBlog == null)
            {
                return NotFound($"Blog with ID {id} not found.");
            }

            await repo.DeleteBlogAsync(id);
            return NoContent();
        }
    }
}

















//using blogsWebApi.Data;
//using blogsWebApi.Models;
//using Microsoft.AspNetCore.Mvc;
//using Microsoft.EntityFrameworkCore;

////using blogsWebApi.Models;

//namespace blogsWebApi.Controllers
//{
//    [Route("api/[controller]")]
//    [ApiController]
//    public class BlogController : ControllerBase
//    {
//        private readonly DataContext db;

//        public BlogController(DataContext db)
//        {
//            this.db = db;
//        }

//        [HttpGet("")]
//        public async  Task<IActionResult> GetAllBlogs()
//        {

//            var blogs = await db.Blogs.ToListAsync();
//            if (blogs == null || !blogs.Any())
//            {
//                return NotFound("No blogs found.");
//            }
//            return Ok(blogs);

//        }

//        // Create Blog

//        [HttpPost]
//        public async Task<IActionResult> CreateBlog()
//        {
//            var blog = new Blog
//            {
//                Title = "Sample Blog 2",
//                Content = "This is a sample blog content. 2",
//                FeaturedImage = "sample-image.jpg",
//                AuthorId = 2, // Assuming author with ID 1 exists
//                CategoryId = 2 // Assuming category with ID 1 exists
//            };
//            await db.Blogs.AddAsync(blog);
//            await db.SaveChangesAsync();
//            return CreatedAtAction(nameof(GetAllBlogs), new { id = blog.Id }, blog);

//        }


//        [HttpDelete("{id}")]

//        public async Task<IActionResult> DeleteBlog(int id)
//        {
//            var blog = await db.Blogs.FindAsync(id);
//            if (blog == null)
//            {
//                return NotFound($"Blog with ID {id} not found.");
//            }
//            db.Blogs.Remove(blog);
//            await db.SaveChangesAsync();
//            return NoContent();
//        }
//    }
//}