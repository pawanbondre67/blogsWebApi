using blogApi.Models;
using blogApi.Repositories;
using blogApi.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace blogApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriesController : ControllerBase
    {
        private readonly ICategoryRepository _repository;

        public CategoriesController(ICategoryRepository repository)
        {
            _repository = repository;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var categories = await _repository.GetAllAsync();
            var result = categories.Select(c => new CategoryDto
            {
                Id = c.Id,
                Name = c.Name
            });
            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var category = await _repository.GetByIdAsync(id);
            if (category == null) return NotFound();
            return Ok(new CategoryDto { Id = category.Id, Name = category.Name });
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateCategoryDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (await _repository.CategoryExistsAsync(dto.Name))
                return Conflict(new { Message = $"Category '{dto.Name}' already exists." });

            var category = new Category
            {
                Name = dto.Name
            };
            await _repository.AddAsync(category);
            var categoryDto = new CategoryDto
            {
                Id = category.Id,
                Name = category.Name,
                PostCount = 0
            };
            return CreatedAtAction(nameof(GetById), new { id = category.Id }, categoryDto);
        }

        [Authorize]
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateCategoryDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (id != dto.Id)
                return BadRequest(new { Message = "ID mismatch." });

            var category = await _repository.GetByIdAsync(id);
            if (category == null)
                return NotFound(new { Message = $"Category with ID {id} not found." });

            if (await _repository.CategoryExistsAsync(dto.Name) && category.Name.ToLower() != dto.Name.ToLower())
                return Conflict(new { Message = $"Category '{dto.Name}' already exists." });

            category.Name = dto.Name;
            await _repository.UpdateAsync(category);
            return NoContent();
        }

        [Authorize]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var category = await _repository.GetByIdAsync(id);
            if (category == null)
                return NotFound(new { Message = $"Category with ID {id} not found." });

            if (category.BlogPosts.Any())
                return BadRequest(new { Message = "Cannot delete category with associated posts." });

            await _repository.DeleteAsync(id);
            return NoContent();
        }
    }
}