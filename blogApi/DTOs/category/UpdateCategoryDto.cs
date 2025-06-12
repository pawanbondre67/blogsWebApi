using System.ComponentModel.DataAnnotations;

namespace blogApi.DTOs.category
{
    public class UpdateCategoryDto
    {
        [Required(ErrorMessage = "Id is required")]
        public int Id { get; set; }

        [Required(ErrorMessage = "Name is required")]
        [StringLength(50, ErrorMessage = "Name cannot exceed 50 characters")]
        public string Name { get; set; }
    }
}
