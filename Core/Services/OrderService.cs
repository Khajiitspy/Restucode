using AutoMapper;
using Core.Interface;
using Core.Models.Order;
using Core.Models.General;
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
            .Include(o => o.OrderStatus)
            .Include(o => o.DeliveryInfo)
                .ThenInclude(i => i.City)
            .Include(o => o.DeliveryInfo)
                .ThenInclude(i => i.PostDepartment)
            .Include(o => o.DeliveryInfo)
                .ThenInclude(i => i.PaymentType)
            .Where(o => o.UserId == userId)
            .ToListAsync();

        return mapper.Map<List<OrderModel>>(orders);
    }

    public async Task<OrderOptions> GetOrderOptions()
    {
        var cities = await context.Cities
            .ToListAsync();

        var postDepartments = await context.PostDepartments
            .ToListAsync();

        var paymentTypes = await context.PaymentTypes
            .ToListAsync();

        var OrderOp = new OrderOptions {
            Cities = mapper.Map<List<SimpleModel>>(cities),
            PostDepartments = mapper.Map<List<SimpleModel>>(postDepartments),
            PaymentTypes = mapper.Map<List<SimpleModel>>(paymentTypes),
        };

        return OrderOp;
    }
}
