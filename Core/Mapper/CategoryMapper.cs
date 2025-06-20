using AutoMapper;
using Domain.Entities;
using Core.Models.Category;
using Core.Models.Seeder;
using Slugify;

namespace Core.Mapper
{
    public class CategoryMapper: Profile
    {
        public CategoryMapper()
        {
            CreateMap<CategoryEntity,CategoryItemViewModel>();
            CreateMap<SeederCategoryModel, CategoryEntity>();

            var slugHelper = new SlugHelper();

            CreateMap<CategoryAddModel, CategoryEntity>()
                .ForMember(dest=> dest.Name, opt=> opt.MapFrom(src=> src.Name.Trim()))
                .ForMember(dest => dest.Slug, opt => opt.MapFrom(src => slugHelper.GenerateSlug(src.Name)));
            CreateMap<CategoryEditModel, CategoryEntity>()
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name.Trim()))
                .ForMember(dest => dest.Slug, opt => opt.MapFrom(src => slugHelper.GenerateSlug(src.Name)))
                .ForMember(dest => dest.Image, opt => opt.Ignore());
            CreateMap<CategoryEntity, CategoryEditModel>()
                .ForMember(dest => dest.Image, opt => opt.Ignore())
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name));

        }
    }
}
