using AutoMapper;
using blogApi.DTOs.blog;
using blogApi.DTOs.comment;
using blogApi.Models;
using blogApi.Repositories;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace blogApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CommentsController : ControllerBase
    {
        private readonly ICommentRepository _repository;
        private readonly IMapper _mapper;

        public CommentsController(ICommentRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        [HttpGet("post/{postId}")]
        public async Task<IActionResult> GetByPost(int postId)
        {
            var comments = await _repository.FindAsync(c => c.BlogPostId == postId);
            if (comments == null || !comments.Any())
                return NotFound("No comments found for this post.");

            var commentDtos = _mapper.Map<IEnumerable<CommentDto>>(comments);
            return Ok(commentDtos);
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateCommentDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var comment = _mapper.Map<Comment>(dto);
            comment.CreatedAt = DateTime.UtcNow;
            comment.UserId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var savedComment = await _repository.AddAsync(comment);
            var commentDto = _mapper.Map<CommentDto>(savedComment);
            return CreatedAtAction(nameof(GetByPost), new { postId = comment.BlogPostId }, commentDto);

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