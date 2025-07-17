using AutoMapper;
using Core.Models.Order;
using Core.Models.General;
using Domain.Entities;
using Domain.Entities.Delivery;

namespace Core.Mapper;

public class OrderMapper: Profile
{
    public OrderMapper()
    {
        CreateMap<OrderEntity, OrderModel>()
            .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.OrderStatus!.Name))
            .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(src => src.DateCreated))
            .ForMember(dest => dest.Items, opt => opt.MapFrom(src => src.OrderItems))
            .ForMember(dest => dest.City, opt => opt.MapFrom(src => src.DeliveryInfo.City.Name))
            .ForMember(dest => dest.PostDepartment, opt => opt.MapFrom(src => src.DeliveryInfo.PostDepartment.Name))
            .ForMember(dest => dest.PaymentType, opt => opt.MapFrom(src => src.DeliveryInfo.PaymentType.Name))
            .ForMember(dest => dest.PhoneNumber, opt => opt.MapFrom(src => src.DeliveryInfo.PhoneNumber))
            .ForMember(dest => dest.RecipientName, opt => opt.MapFrom(src => src.DeliveryInfo.RecipientName));

        CreateMap<OrderItemEntity, OrderItemModel>()
            .ForMember(dest => dest.ProductVariantName, opt => opt.MapFrom(src => src.ProductVariant!.Name));   

        CreateMap<CityEntity, SimpleModel>();
        CreateMap<PostDepartmentEntity, SimpleModel>();
        CreateMap<PaymentTypeEntity, SimpleModel>();
    }
}
