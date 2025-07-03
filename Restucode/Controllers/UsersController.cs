using Core.Interface;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Core.Models.AdminUser;
using Core.Models.General;
using Restucode.Constants;

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
    public async Task<ActionResult<PagedResult<AdminUserItemModel>>> SearchUsers(
        [FromQuery] string? role,
        [FromQuery] string? fullName,
        [FromQuery] DateTime? startDate,
        [FromQuery] DateTime? endDate,
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 5
        )
    {
        var searchModel = new AdminUserFilterModel
        {
            Role = role,
            FullName = fullName,
            RegisteredFrom = startDate,
            RegisteredTo = endDate,
            Page = page,
            PageSize = pageSize
        };

        var result = await userService.GetFilteredUsersAsync(searchModel);
        return Ok(result);
    }
}
