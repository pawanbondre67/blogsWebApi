using blogApi.Models;
using System.ComponentModel.DataAnnotations;

namespace blogApi.DTOs.comment
{
    public class CommentDto
    {
 
        public int Id { get; set; }
        public string Content { get; set; }
        public DateTime CreatedAt { get; set; }
        public int BlogPostId { get; set; }
        public string UserName { get; set; }

    }
}
