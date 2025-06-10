using blogApi.Models;
using blogApi.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using blogApi.DTOs;
namespace blogApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LikesController : ControllerBase
    {
        private readonly IRepository<Like> _repository;

        public LikesController(IRepository<Like> repository)
        {
            _repository = repository;
        }

        [HttpGet("post/{postId}")]
        public async Task<IActionResult> GetByPost(int postId)
            => Ok(await _repository.FindAsync(l => l.BlogPostId == postId));
                                
        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateLikeDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var userId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
            if (userId == null)
                return Unauthorized();

            var existingLike = await _repository.FindAsync(l => l.BlogPostId == dto.BlogPostId && l.UserId == userId);
            if (existingLike.Any())
                return Conflict(new { Message = "User has already liked this post." });

            var like = new Like
            {
                BlogPostId = dto.BlogPostId,
                UserId = userId
            };
            await _repository.AddAsync(like);
            return CreatedAtAction(nameof(GetByPost), new { postId = like.BlogPostId }, like);
        }

        [Authorize]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var like = await _repository.GetByIdAsync(id);
            if (like == null)
                return NotFound();

            await _repository.DeleteAsync(id);
            return NoContent();
        }
    }
}