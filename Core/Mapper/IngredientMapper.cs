using AutoMapper;
using Domain.Entities;
using Core.Models.Category;
using Core.Models.Seeder;
using Slugify;
using Core.Models.Product;

namespace Core.Mapper
{
    public class IngredientMapper: Profile
    {
        public IngredientMapper()
        {
            CreateMap<SeederIngredientModel, IngredientEntity>();
            CreateMap<IngredientEntity, IngredientModel>()
                .ForMember(x => x.ImageUrl, opt => opt.MapFrom(src => src.Image));
            CreateMap<CreateIngredientModel, IngredientEntity>()
                .ForMember(x => x.Image, opt => opt.Ignore());
        }
    }
}
