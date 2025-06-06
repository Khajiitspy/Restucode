using AutoMapper;
using Domain.Entities;
using Core.Models.Category;
using Core.Models.Seeder;
using Slugify;
using Core.Models.Product;

namespace Core.Mapper
{
    public class ProductSizeMapper: Profile
    {
        public ProductSizeMapper()
        {
            CreateMap<SeederProductSizeModel, ProductSizeEntity>();
            CreateMap<ProductSizeEntity, ProductSizeModel>();
        }
    }
}
