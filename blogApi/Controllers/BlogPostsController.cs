using AutoMapper;
using blogApi.DTOs.blog;
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
        private readonly IMapper _mapper;

        public BlogPostsController(IBlogPostRepository repository , IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] int page = 1, [FromQuery] int pageSize = 10)
        {
            if (page < 1 || pageSize < 1)
                return BadRequest("Invalid page or pageSize.");

            var posts = await _repository.GetPaginatedAsync(page, pageSize);
            var postDtos = _mapper.Map<IEnumerable<BlogPostDto>>(posts);
            return Ok(postDtos);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var post = await _repository.GetByIdAsync(id);
          
            if (post == null)
                return NotFound();

            var postDto = _mapper.Map<BlogPostDto>(post);
            return Ok(postDto);
        }

        [HttpGet("category/{categoryId}")]
        public async Task<IActionResult> GetByCategory(int categoryId, [FromQuery] int page , [FromQuery] int pageSize)
        {
            if (page < 1 || pageSize < 1)
                return BadRequest("Invalid page or pageSize.");

            var posts = await _repository.GetByCategoryPaginatedAsync(categoryId, page, pageSize);
            if (posts == null || !posts.Any())
                return NotFound("No posts found for this category.");

            var postDtos = _mapper.Map<IEnumerable<BlogPostDto>>(posts);
            return Ok(postDtos);
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateBlogPostDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var post = _mapper.Map<BlogPost>(dto);
            post.CreatedAt = DateTime.UtcNow;
            post.UpdatedAt = DateTime.UtcNow;
            post.AuthorId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value // Get the current user's ID
            ?? throw new InvalidOperationException("User ID not found in claims.");

            var savedPost = await _repository.AddAsync(post);
            var postDto = _mapper.Map<BlogPostDto>(savedPost);
            return CreatedAtAction(nameof(GetById), new { id = post.Id }, postDto);

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