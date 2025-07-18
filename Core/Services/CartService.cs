using System.Net.Mail;
using System.Net.Http.Headers;
using Core.SMTP;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Core.Interface;
using Core.Models.Cart;
using Core.Models.Order;
using Domain;
using Domain.Entities;
using Domain.Entities.Delivery;
using Microsoft.EntityFrameworkCore;

namespace Core.Services;

public class CartService(RestucodeDBContext context, IAuthService authservice, IMapper mapper, ISmtpService smtpService): ICartService
{
    public async Task CreateUpdate(CartCreateUpdateModel model)
    {
        var userId = await authservice.GetuserId();
        var entity = context.Carts
            .SingleOrDefault(x => x.UserId == userId && x.ProductVariantId == model.ProductVariantId);
        if (entity != null)
        {
            entity.Quantity += model.Quantity;
            if (entity.Quantity <= 0)
            {
                context.Carts.Remove(entity);
            }
        }
        else
        {
            entity = new CartEntity
            {
                UserId = userId,
                ProductVariantId = model.ProductVariantId,
                Quantity = model.Quantity
            };
            context.Carts.Add(entity); }
        await context.SaveChangesAsync();
    }
    
    public async Task RemoveFromCart(long productVariantId)
    {
        var userId = await authservice.GetuserId();
        var entity = context.Carts
            .SingleOrDefault(x => x.UserId == userId && x.ProductVariantId == productVariantId);
        if (entity != null)
        {
            context.Carts.Remove(entity);
            await context.SaveChangesAsync();
        }
    }

    public async Task<List<CartItemModel>> GetCartItems()
    {
        var userId = await authservice.GetuserId();
        Console.WriteLine("USER - ID: " + userId + "--------------------");

        var items = await context.Carts
            .Where(x => x.UserId == userId)
            .ProjectTo<CartItemModel>(mapper.ConfigurationProvider)
            .ToListAsync();
        
        return items;
    }

    public async Task OrderCart(OrderInformation info){
        var orderStatusId = new Random().Next(3,15);
        var userId = await authservice.GetuserId();

        var order = new OrderEntity{
            OrderStatusId = orderStatusId,
            UserId = userId,
            OrderItems = new List<OrderItemEntity>{},
            DeliveryInfo = new DeliveryInfoEntity{
                CityId = info.CityId,
                PostDepartmentId = info.PostDepartmentId,
                PaymentTypeId = info.PaymentTypeId,
                PhoneNumber = info.PhoneNumber,
                RecipientName = info.RecipientName
            }
        };

        context.Orders.Add(order);
        await context.SaveChangesAsync();

        order.DeliveryInfo.OrderId = order.Id;

        var items = await context.Carts
            .Include(x => x.ProductVariant)
            .Where(x => x.UserId == userId)
            .ToListAsync();

        decimal totalPrice = 0;

        items.ForEach(x => {
            context.OrderItems.Add(new OrderItemEntity{
                PriceBuy = x.ProductVariant.Price,
                Count = x.Quantity,
                ProductVariantId = x.ProductVariantId,
                OrderId = order.Id,
            });
            totalPrice += x.ProductVariant.Price * x.Quantity;
        });

        await context.SaveChangesAsync();

        items.ForEach(x => {
            context.Carts.Remove(x);
        });

        await context.SaveChangesAsync();

        var emailModel = new EmailMessage
        {
            To = info.Email,
            Subject = "New Order For " + info.RecipientName,
            Body = $"<p>Ordered {order.OrderItems.Count()} items for ${totalPrice}"
        };

        await smtpService.SendEmailAsync(emailModel);
    }
}
