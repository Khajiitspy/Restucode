using AutoMapper;
using AutoMapper.QueryableExtensions;
using Core.Interface;
using Core.Models.Cart;
using Domain;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Core.Services;

public class CartService(RestucodeDBContext context, IAuthService authservice, IMapper mapper): ICartService
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
}
