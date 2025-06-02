using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace blogsWebApi.Models
{
    public class Blog
    {
        public int Id { get; set; }

        [Required]
        public string Title { get; set; }

        [Required]
        public string Content { get; set; }

        [Required]
        public string FeaturedImage { get; set; }

        public int AuthorId { get; set; }
        public User Author { get; set; }

        public int CategoryId { get; set; }
        public Category Category { get; set; }

        public int Views { get; set; } = 0;

        public ICollection<User> LikedByUsers { get; set; }
        public ICollection<Comment> Comments { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
