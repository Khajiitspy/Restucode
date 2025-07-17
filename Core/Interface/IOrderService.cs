using System.Security.Claims;
using Core.Models.Order;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Core.Interface;

public interface IOrderService
{
    public Task<List<OrderModel>> GetUserOrders();
    public Task<OrderOptions> GetOrderOptions();
}
