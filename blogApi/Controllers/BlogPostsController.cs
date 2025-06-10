using blogApi.DTOs;
using blogApi.Models;
using blogApi.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace blogApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BlogPostsController : ControllerBase
    {
        private readonly IBlogPostRepository _repository;

        public BlogPostsController(IBlogPostRepository repository)
        {
            _repository = repository;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] int page = 1, [FromQuery] int pageSize = 10)
            => Ok(await _repository.GetPaginatedAsync(page, pageSize));

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var post = await _repository.GetByIdAsync(id);
            return post != null ? Ok(post) : NotFound();
        }

        [HttpGet("category/{categoryId}")]
        public async Task<IActionResult> GetByCategory(int categoryId)
            => Ok(await _repository.GetByCategoryAsync(categoryId));

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateBlogPostDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var post = new BlogPost
            {
                Title = dto.Title,
                Content = dto.Content,
                CategoryId = dto.CategoryId,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow,
                AuthorId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value
            };
            await _repository.AddAsync(post);
            return CreatedAtAction(nameof(GetById), new { id = post.Id }, post);
        }

        [Authorize]
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateBlogPostDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (id != dto.Id)
                return BadRequest();

            var post = await _repository.GetByIdAsync(id);
            if (post == null)
                return NotFound();

            post.Title = dto.Title;
            post.Content = dto.Content;
            post.CategoryId = dto.CategoryId;
            post.UpdatedAt = DateTime.UtcNow;
            await _repository.UpdateAsync(post);
            return NoContent();
        }

        [Authorize]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var post = await _repository.GetByIdAsync(id);
            if (post == null)
                return NotFound();

            await _repository.DeleteAsync(id);
            return NoContent();
        }
    }
}