using AutoMapper;
using Core.Models.Cart;
using Domain.Entities;

namespace Core.Mapper;

public class CartMapper: Profile
{
    public CartMapper()
    {
        CreateMap<CartEntity, CartItemModel>()
            .ForMember(x => x.ProductVariantId, opt => opt.MapFrom(x => x.ProductVariant.Id))
            .ForMember(x => x.CategoryId, opt => opt.MapFrom(x => x.ProductVariant!.Category!.Id))
            .ForMember(x => x.CategoryName, opt => opt.MapFrom(x => x.ProductVariant!.Category!.Name))
            .ForMember(x => x.Name, opt => opt.MapFrom(x => x.ProductVariant!.Name))
            .ForMember(x => x.Price, opt => opt.MapFrom(x => x.ProductVariant!.Price))
            .ForMember(x => x.ImageName, opt => opt.MapFrom(x =>
                x.ProductVariant!.ProductImages != null && x.ProductVariant.ProductImages.Any()
                    ? x.ProductVariant.ProductImages.OrderBy(x => x.Priority).First().Name
                    : null));
    }
}