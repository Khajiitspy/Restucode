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

    public async Task<List<SimpleModel>> GetCities(string? search = null)
    {
        if(search == null || search == ""){
            Console.WriteLine("search Is NUll!!!!!");
            var cities = await context.Cities
                .OrderBy(c => c.Id)
                .ToListAsync();

            return mapper.Map<List<SimpleModel>>(cities);
        }
        else{
            var cities = await context.Cities
                .Where(c => c.Name.ToLower().Contains(search.ToLower()))
                .OrderBy(c => c.Name.ToLower().IndexOf(search.ToLower()))
                .ToListAsync();

            return mapper.Map<List<SimpleModel>>(cities);
        }
    }

    public async Task<List<SimpleModel>> GetPostDepartments(long cityId)
    {
        var postDepartments = await context.PostDepartments
            .Where(d => d.CityId == cityId)
            .ToListAsync();

        return mapper.Map<List<SimpleModel>>(postDepartments);
    }

    public async Task<List<SimpleModel>> GetPaymentTypes()
    {
        var paymentTypes = await context.PaymentTypes
            .ToListAsync();

        return mapper.Map<List<SimpleModel>>(paymentTypes);
    }
}
