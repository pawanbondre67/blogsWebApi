using System.ComponentModel.DataAnnotations;

namespace blogsWebApi.Models
{
    public class Comment
    {
        public int Id { get; set; }

        [Required]
        public string Text { get; set; }

        public int BlogId { get; set; }
        public Blog Blog { get; set; }

        public int UserId { get; set; }
        public User User { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
