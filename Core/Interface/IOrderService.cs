using System.Security.Claims;
using Core.Models.Order;
using Core.Models.General;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Core.Interface;

public interface IOrderService
{
    public Task<List<OrderModel>> GetUserOrders();
    // public Task<OrderOptions> GetOrderOptions();
    public Task<List<SimpleModel>> GetCities(string search = null);
    public Task<List<SimpleModel>> GetPostDepartments(long cityId);
    public Task<List<SimpleModel>> GetPaymentTypes();
}
