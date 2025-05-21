using AutoMapper;
using Restucode.Data.Entities;
using Restucode.Models.Category;
using Restucode.Models.Seeder;
using Slugify;

namespace Restucode.Mapper
{
    public class CategoryMapper: Profile
    {
        public CategoryMapper()
        {
            CreateMap<CategoryEntity,CategoryItemViewModel>();
            CreateMap<SeederCategoryModel, CategoryEntity>();

            var slugHelper = new SlugHelper();

            CreateMap<CategoryAddModel, CategoryEntity>()
                .ForMember(dest => dest.Slug, opt => opt.MapFrom(src => slugHelper.GenerateSlug(src.Name)));
            CreateMap<CategoryEditModel, CategoryEntity>()
                .ForMember(dest => dest.Slug, opt => opt.MapFrom(src => slugHelper.GenerateSlug(src.Name)))
                .ForMember(dest => dest.Image, opt => opt.Ignore());
            CreateMap<CategoryEntity, CategoryEditModel>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.Image, opt => opt.Ignore())
                .ForMember(dest => dest.ViewImage, opt => opt.MapFrom(src => src.Image))
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name));

        }
    }
}
