namespace blogApi.DTOs
{
    public class LikeDto
    {
        public int Id { get; set; }
        public int BlogPostId { get; set; }
        public string UserId { get; set; }
        public string UserName { get; set; }
    }
}
