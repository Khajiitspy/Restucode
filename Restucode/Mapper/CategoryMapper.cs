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
        }
    }
}
