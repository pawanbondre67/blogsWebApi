using AutoMapper;
using blogApi.DTOs.blog;
using blogApi.DTOs.comment;
using blogApi.Models;


public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<BlogPost, BlogPostDto>()
            .ForMember(dest => dest.CategoryName, opt => opt.MapFrom(src => src.Category.Name))
            .ForMember(dest => dest.AuthorName, opt => opt.MapFrom(src => src.Author.FullName)) // Assuming ApplicationUser has FullName
            .ForMember(dest => dest.CommentCount, opt => opt.MapFrom(src => src.Comments != null ? src.Comments.Count : 0))
            .ForMember(dest => dest.LikeCount, opt => opt.MapFrom(src => src.Likes != null ? src.Likes.Count : 0));
        CreateMap<CreateBlogPostDto, BlogPost>();


        CreateMap<Comment, CommentDto>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
            .ForMember(dest => dest.Content, opt => opt.MapFrom(src => src.Content))
            .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(src => src.CreatedAt))
            .ForMember(dest => dest.BlogPostId, opt => opt.MapFrom(src => src.BlogPostId))
            .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.User.FullName));

        CreateMap<CreateCommentDto, Comment>();
    }
}