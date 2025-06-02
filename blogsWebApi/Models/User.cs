using System.ComponentModel.DataAnnotations;

namespace blogsWebApi.Models
{
    public class User
    {
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        [Required, EmailAddress]
        public string Email { get; set; }

        public string Password { get; set; }

        public string Role { get; set; } = "user"; // "user" or "admin"

        public string? SocialProvider { get; set; }

        public ICollection<Blog> Blogs { get; set; }

        public ICollection<Comment> Comments { get; set; }

        public ICollection<Blog> LikedBlogs { get; set; }
    }
}
