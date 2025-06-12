using System.ComponentModel.DataAnnotations;

namespace blogApi.DTOs.blog
{
    public class BlogPostDto
    {
        public int Id { get; set; }

        public required string Title { get; set; }
        
        public required string Content { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        //public int CategoryId { get; set; }
        public  string CategoryName { get; set; }
        //public string AuthorId { get; set; }
        public  string AuthorName { get; set; }
        public int CommentCount { get; set; }
        public int LikeCount { get; set; }
    }
}
