using AutoMapper;
using Core.Models.Order;
using Domain.Entities;

namespace Core.Mapper;

public class OrderMapper: Profile
{
    public OrderMapper()
    {
        CreateMap<OrderEntity, OrderModel>()
            .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.OrderStatus!.Name))
            .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(src => src.DateCreated))
            .ForMember(dest => dest.Items, opt => opt.MapFrom(src => src.OrderItems));

        CreateMap<OrderItemEntity, OrderItemModel>()
            .ForMember(dest => dest.ProductVariantName, opt => opt.MapFrom(src => src.ProductVariant!.Name));   
    }
}