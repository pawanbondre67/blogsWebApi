using blogApi.DTOs;
using blogApi.Models;
using blogApi.Repositories;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace blogApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CommentsController : ControllerBase
    {
        private readonly IRepository<Comment> _repository;

        public CommentsController(IRepository<Comment> repository)
        {
            _repository = repository;
        }

        [HttpGet("post/{postId}")]
        public async Task<IActionResult> GetByPost(int postId)
            => Ok(await _repository.FindAsync(c => c.BlogPostId == postId));

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateCommentDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var comment = new Comment
            {
                Content = dto.Content,
                BlogPostId = dto.BlogPostId,
                CreatedAt = DateTime.UtcNow,
                UserId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value
            };
            await _repository.AddAsync(comment);
            return CreatedAtAction(nameof(GetByPost), new { postId = comment.BlogPostId }, comment);
        }

        [Authorize]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var comment = await _repository.GetByIdAsync(id);
            if (comment == null)
                return NotFound();

            await _repository.DeleteAsync(id);
            return NoContent();
        }
    }
}