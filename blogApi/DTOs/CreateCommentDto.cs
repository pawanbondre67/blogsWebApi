namespace blogApi.DTOs
{
    public class CreateCommentDto
    {
        public string Content { get; set; }
        public int BlogPostId { get; set; }
    }
}
