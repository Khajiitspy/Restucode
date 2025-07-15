﻿using AutoMapper;
using Domain.Entities.Identity;
using Core.Models.Seeder;
using Core.Models.Account;

namespace Core.Mapper
{
    public class UserMapper: Profile
    {
        public UserMapper()
        {
            CreateMap<SeederUserModel, UserEntity>()
                .ForMember(opt => opt.UserName, opt => opt.MapFrom(x => x.Email));
            CreateMap<RegisterModel, UserEntity>()
                .ForMember(opt => opt.UserName, opt => opt.MapFrom(x => x.Email));

            CreateMap<GoogleAccountModel, UserEntity>()
                .ForMember(x => x.Image, opt => opt.Ignore())
                .ForMember(x => x.UserName, opt => opt.MapFrom(x => x.Email));

            CreateMap<UserEntity, UserProfileModel>()
                .ForMember(x => x.Avatar, opt => opt.MapFrom(x => x.Image));
        }
    }
}
