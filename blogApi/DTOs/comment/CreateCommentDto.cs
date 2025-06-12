using System.ComponentModel.DataAnnotations;

namespace blogApi.DTOs.comment
{
    public class CreateCommentDto
    {

        [Required]
        public required string Content { get; set; }
        public int BlogPostId { get; set; }
    }
}
