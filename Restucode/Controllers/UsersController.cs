using Core.Interface;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Core.Models.AdminUser;
using Core.Models.General;
using Core.Models.Seeder;
using Core.Constants;

namespace Restucode.Controllers;

[Route("api/[controller]")]
[ApiController]
public class UsersController(IUserService userService) : Controller
{
    [HttpGet("list")]
    public async Task<IActionResult> GetAllUsers()
    {
        var model = await userService.GetAllUsersAsync();

        return Ok(model);
    }

    [HttpGet("search")]
    [Authorize(Roles = Roles.Admin)]
    public async Task<ActionResult<PagedResult<AdminUserItemModel>>> SearchUsers([FromQuery] AdminUserFilterModel model)
    {
        var result = await userService.GetFilteredUsersAsync(model);
        return Ok(result);
    }

    [HttpGet("seed")]
    public async Task<IActionResult> SeedUsers([FromQuery] SeedItemsModel model)
    {
        var result = await userService.SeedAsync(model);
        return Ok(result);
    }

    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Edit([FromForm] UserEditModel request)
    {
        await userService.EditAsync(request);
        return Ok();
    }

    [Authorize(Roles = "Admin")]
    [HttpGet("{id}")]
    public async Task<ActionResult<UserEditViewModel>> GetById(long id)
    {
        var user = await userService.GetByIdAsync(id);
        return Ok(user);
    }

    [Authorize(Roles = "Admin")]
    [HttpGet("roles")]
    public async Task<ActionResult<List<string>>> GetAllRoles()
    {
        var roles = await userService.GetAllRoles();
        return Ok(roles);
    }
}
