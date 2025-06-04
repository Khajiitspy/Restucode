using AutoMapper;
using Core.Models.Product;
using Domain.Entities;
using System.Linq;

namespace Core.Mapper
{
    public class ProductMapper : Profile
    {
        public ProductMapper()
        {
            CreateMap<ProductEntity, ProductItemViewModel>()
                .ForMember(dest => dest.Name,
                    opt => opt.MapFrom(src => src.ProductVariants
                        .OrderBy(pv => pv.Id)
                        .Select(pv => pv.Name)
                        .FirstOrDefault()))
                .ForMember(dest => dest.Price,
                    opt => opt.MapFrom(src => src.ProductVariants
                        .OrderBy(pv => pv.Id)
                        .Select(pv => pv.Price)
                        .FirstOrDefault()))
                .ForMember(dest => dest.Image,
                    opt => opt.MapFrom(src => src.ProductVariants
                        .SelectMany(pv => pv.ProductImages)
                        .OrderBy(pi => pi.Priority)
                        .Select(pi => pi.Name)
                        .FirstOrDefault()));

            // For product details page
            CreateMap<ProductEntity, ProductDetailsViewModel>()
                .ForMember(dest => dest.Variants,
                    opt => opt.MapFrom(src => src.ProductVariants ?? new List<ProductVariantEntity>()));

            // For each variant in the details view
            CreateMap<ProductVariantEntity, ProductVariant>()
                .ForMember(dest => dest.Category,
                    opt => opt.MapFrom(src => src.Category != null ? src.Category.Name : string.Empty))
                .ForMember(dest => dest.Size,
                    opt => opt.MapFrom(src => src.ProductSize != null ? src.ProductSize.Name : null))
                .ForMember(dest => dest.Ingredients,
                    opt => opt.MapFrom(src =>
                        (src.ProductIngredients ?? new List<ProductIngredientEntity>())
                        .Select(pi => pi.Ingredient)
                        .Where(i => i != null)
                        .Select(i => new IngredientModel
                        {
                            Name = i!.Name,
                            ImageUrl = i.Image
                        })
                        .ToList()))
                .ForMember(dest => dest.Images,
                    opt => opt.MapFrom(src =>
                        (src.ProductImages ?? new List<ProductImageEntity>())
                        .OrderBy(pi => pi.Priority)
                        .Select(pi => pi.Name)
                        .ToList()));
        }
    }
}

