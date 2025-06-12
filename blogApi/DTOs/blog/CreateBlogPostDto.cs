using System.ComponentModel.DataAnnotations;

namespace blogApi.DTOs.blog
{
    public class CreateBlogPostDto
    {
        [Required]
        [StringLength(200)]
        public string Title { get; set; }

        [Required]
        public string Content { get; set; }

        [Required]
        public int CategoryId { get; set; }

    }
}