using AutoMapper;
using Phorum.Entities;
using Phorum.Models;

namespace Phorum
{
    public class PhorumProfile:Profile
    {
        public PhorumProfile()
        {
            CreateMap<User, UserDTO>().ReverseMap();
            CreateMap<Post, PostDTO>()
            .ForMember(dest => dest.User, opt => opt.MapFrom(src => new UserDTO
            {
                Id = src.User.Id,
                Name = src.User.Name,
                Email = src.User.Email,
            })).ReverseMap();
        }
    }
}
