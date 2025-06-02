using AutoMapper;
using Core.Models.Product;
using Domain.Entities;

namespace Core.Mapper
{
    public class ProductMapper: Profile
    {
        public ProductMapper()
        {
            CreateMap<ProductEntity, ProductItemViewModel>()
            .ForMember(dest => dest.Image,
                opt => opt.MapFrom(src => src.ProductImages!
                    .OrderBy(pi => pi.Priority)
                    .Select(pi => pi.Name)
                    .FirstOrDefault() ?? string.Empty));

            CreateMap<ProductEntity, ProductDetailsViewModel>()
                .ForMember(dest => dest.Category, opt => opt.MapFrom(src => src.Category!.Name))
                .ForMember(dest => dest.Size, opt => opt.MapFrom(src => src.ProductSize != null ? src.ProductSize.Name : null))
                .ForMember(dest => dest.Ingredients, opt => opt.MapFrom(src => src.ProductIngredients!
                    .Select(pi => pi.Ingredient!.Name).ToList()))
                .ForMember(dest => dest.Images, opt => opt.MapFrom(src => src.ProductImages!
                    .OrderBy(pi => pi.Priority)
                    .Select(pi => pi.Name)
                    .ToList()));
        }
    }
}
