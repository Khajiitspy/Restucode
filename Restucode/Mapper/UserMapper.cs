using AutoMapper;
using Restucode.Data.Entities.Identity;
using Restucode.Models.Seeder;
using Restucode.Models.Account;

namespace Restucode.Mapper
{
    public class UserMapper: Profile
    {
        public UserMapper()
        {
            CreateMap<SeederUserModel, UserEntity>()
                .ForMember(opt => opt.UserName, opt => opt.MapFrom(x => x.Email));
            CreateMap<RegisterModel, UserEntity>()
                .ForMember(opt => opt.UserName, opt => opt.MapFrom(x => x.Email));

        }
    }
}
