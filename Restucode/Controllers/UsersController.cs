using Core.Interface;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Core.Models.AdminUser;
using Core.Models.General;
using Core.Models.Seeder;
using Core.Constants;
using System.Diagnostics;

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
        Stopwatch stopWatch = new Stopwatch();
        stopWatch.Start();
        var result = await userService.GetFilteredUsersAsync(model);
        stopWatch.Stop();
        // Get the elapsed time as a TimeSpan value.
        TimeSpan ts = stopWatch.Elapsed;

        // Format and display the TimeSpan value.
        string elapsedTime = String.Format("{0:00}:{1:00}:{2:00}.{3:00}",
            ts.Hours, ts.Minutes, ts.Seconds,
            ts.Milliseconds / 10);
        Console.WriteLine("-----------Elapsed Time------------: " + elapsedTime);
        return Ok(result);
    }

    // [HttpGet("search")]
    // [Authorize(Roles = Roles.Admin)]
    // public async Task<ActionResult<PagedResult<AdminUserItemModel>>> SearchUsers([FromQuery] AdminUserFilterModel model)
    // {
    //     var result = await userService.GetFilteredUsersAsync(model);
    //     return Ok(result);
    // }

    [HttpGet("seed")]
    public async Task<IActionResult> SeedUsers([FromQuery] SeedItemsModel model)
    {
        var result = await userService.SeedAsync(model);
        return Ok(result);
    }

    [Authorize(Roles = "Admin")]
    [HttpPut]
    public async Task<IActionResult> Edit([FromForm] UserEditModel request)
    {
        Stopwatch stopWatch = new Stopwatch();
        stopWatch.Start();
        await userService.EditAsync(request);
        stopWatch.Stop();
        // Get the elapsed time as a TimeSpan value.
        TimeSpan ts = stopWatch.Elapsed;

        // Format and display the TimeSpan value.
        string elapsedTime = String.Format("{0:00}:{1:00}:{2:00}.{3:00}",
            ts.Hours, ts.Minutes, ts.Seconds,
            ts.Milliseconds / 10);
        Console.WriteLine("-----------Elapsed Time------------: " + elapsedTime);

        return Ok();
    }

    [Authorize(Roles = "Admin")]
    [HttpGet("{id}")]
    public async Task<ActionResult<UserEditViewModel>> GetById(long id)
    {
        Stopwatch stopWatch = new Stopwatch();
        stopWatch.Start();
        var user = await userService.GetByIdAsync(id);
                stopWatch.Stop();
        // Get the elapsed time as a TimeSpan value.
        TimeSpan ts = stopWatch.Elapsed;

        // Format and display the TimeSpan value.
        string elapsedTime = String.Format("{0:00}:{1:00}:{2:00}.{3:00}",
            ts.Hours, ts.Minutes, ts.Seconds,
            ts.Milliseconds / 10);
        Console.WriteLine("-----------Elapsed Time------------: " + elapsedTime);

        return Ok(user);
    }

    [Authorize(Roles = "Admin")]
    [HttpGet("roles")]
    public async Task<ActionResult<List<string>>> GetAllRoles()
    {
        Stopwatch stopWatch = new Stopwatch();
        stopWatch.Start();
        var roles = await userService.GetAllRoles();
                stopWatch.Stop();
        // Get the elapsed time as a TimeSpan value.
        TimeSpan ts = stopWatch.Elapsed;

        // Format and display the TimeSpan value.
        string elapsedTime = String.Format("{0:00}:{1:00}:{2:00}.{3:00}",
            ts.Hours, ts.Minutes, ts.Seconds,
            ts.Milliseconds / 10);
        Console.WriteLine("-----------Elapsed Time------------: " + elapsedTime);

        return Ok(roles);
    }
}
