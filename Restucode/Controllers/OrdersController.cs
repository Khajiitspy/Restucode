using System.Security.Claims;
using Core.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace Restucode.Controllers;

[Route("api/[controller]")]
[ApiController]
// [Authorize]
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

    [HttpGet("cities")]
    public async Task<IActionResult> GetCities([FromQuery] string? search = null){
        var cities = await orderService.GetCities(search);
        return Ok(cities);
    }

    [HttpGet("postDepartments")]
    public async Task<IActionResult> GetPostDepartments([FromQuery] long cityId){
        var postDepartments = await orderService.GetPostDepartments(cityId);
        return Ok(postDepartments);
    }

    [HttpGet("paymentTypes")]
    public async Task<IActionResult> GetPaymentTypes(){
        var paymentTypes = await orderService.GetPaymentTypes();
        return Ok(paymentTypes);
    }
}
