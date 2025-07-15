using System.Security.Claims;
using Core.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace Restucode.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class OrdersController(IOrderService orderService): ControllerBase
{
    [Authorize]
    [HttpGet]
    public async Task<IActionResult> GetUserOrders()
    {
        var orders = await orderService.GetUserOrders();
        Console.WriteLine("<------" + orders + "------>");
        return Ok(orders);
    }
}
