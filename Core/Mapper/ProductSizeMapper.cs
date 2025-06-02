using AutoMapper;
using Domain.Entities;
using Core.Models.Category;
using Core.Models.Seeder;
using Slugify;

namespace Core.Mapper
{
    public class ProductSizeMapper: Profile
    {
        public ProductSizeMapper()
        {
            CreateMap<SeederProductSizeModel, ProductSizeEntity>();
        }
    }
}
