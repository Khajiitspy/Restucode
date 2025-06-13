using Core.Interface;
using Core.Models.Cart;
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

    [HttpGet]
    public async Task<IActionResult> GetCartItems()
    {
        var model = await cartService.GetCartItems();
        return Ok(model);
    }
}