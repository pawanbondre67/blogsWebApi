using System.ComponentModel.DataAnnotations;

namespace blogsWebApi.Models
{
    public class Category
    {
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        public string Slug { get; set; }

        public ICollection<Blog> Blogs { get; set; }
    }
}
