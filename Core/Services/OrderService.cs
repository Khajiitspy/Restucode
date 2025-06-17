using AutoMapper;
using Core.Interface;
using Core.Models.Order;
using Domain;
using Microsoft.EntityFrameworkCore;

namespace Core.Services;

public class OrderService(RestucodeDBContext context, IAuthService authservice, IMapper mapper): IOrderService
{
    public async Task<List<OrderModel>> GetUserOrders()
    {
        var userId = await authservice.GetuserId();
        var orders = await context.Orders
            .Include(o => o.OrderItems)
            .ThenInclude(i => i.ProductVariant)
            .Where(o => o.UserId == userId)
            .ToListAsync();

        return mapper.Map<List<OrderModel>>(orders);
    }
}