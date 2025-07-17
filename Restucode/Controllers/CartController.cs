using Core.Interface;
using Core.Models.Cart;
using Core.Models.Order;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace Restucode.Controllers;

[Route("api/[controller]/[action]")]
[ApiController]
[Authorize]
public class CartController(ICartService cartService) : ControllerBase
{
    [HttpPost]
    public async Task<IActionResult> CreateUpdate([FromBody] CartCreateUpdateModel model)
    {
        await cartService.CreateUpdate(model);
        return Ok();
    }

    [Authorize]
    [HttpPost]
    public async Task<IActionResult> AddRange([FromBody] List<CartCreateUpdateModel> modelItems)
    {
        foreach (var item in modelItems)
        {
            await cartService.CreateUpdate(item);
        }
        return Ok();
    }
    
    [HttpDelete("{id}")]
    public async Task<IActionResult> RemoveFromCart(long id)
    {
        await cartService.RemoveFromCart(id);
        return Ok();
    }

    [HttpGet]
    public async Task<IActionResult> GetCartItems()
    {
        var model = await cartService.GetCartItems();
        return Ok(model);
    }

    [Authorize]
    [HttpPost]
    public async Task<IActionResult> OrderCart([FromBody] OrderInformation info){
        await cartService.OrderCart(info);
        return Ok();
    }
}
