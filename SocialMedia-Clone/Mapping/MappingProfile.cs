using AutoMapper;
using Brainrot.Models.Domain;
using Brainrot.Models.Dto;

namespace Brainrot.Mapping;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        //Image Mapping
        CreateMap<Image, ImageResponseDto>().ReverseMap();
        CreateMap<Image, ImageUploadRequestDto>().ReverseMap();
        
        //User Mapping
        CreateMap<User, UserResponseDto>().ReverseMap();
        CreateMap<User, UserRequestDto>().ReverseMap();
        
        //Post Mapping
        CreateMap<Post, PostResponseDto>().ReverseMap();
        CreateMap<Post, PostRequestDto>().ReverseMap();
        CreateMap<Post, PostUpdateDto>().ReverseMap();
        
        //Comment Mapping
        CreateMap<Comment, CommentResponseDto>().ReverseMap();
        CreateMap<Comment, CommentRequestDto>().ReverseMap();
        CreateMap<Comment, CommentUpdateDto>().ReverseMap();
        
        //Like Mapping
        CreateMap<Like, LikeResponseDto>().ReverseMap();
        CreateMap<Like, LikeRequestDto>().ReverseMap();
    }
}