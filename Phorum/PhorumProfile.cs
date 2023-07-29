﻿using AutoMapper;
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
                .ForMember(p => p.User, map => map.MapFrom(post => post.User));
        }
    }
}
