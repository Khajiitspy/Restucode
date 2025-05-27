using AutoMapper;
using Restucode.Data.Entities.Identity;
using Restucode.Models.Seeder;

namespace Restucode.Mapper
{
    public class UserMapper: Profile
    {
        public UserMapper()
        {
            CreateMap<SeederUserModel, UserEntity>()
                .ForMember(opt => opt.UserName, opt => opt.MapFrom(x => x.Email));

        }
    }
}
