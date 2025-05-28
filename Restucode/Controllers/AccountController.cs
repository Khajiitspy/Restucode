using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Restucode.Data.Entities.Identity;
using Restucode.Interface;
using Restucode.Models.Account;
using System.Security.Claims;
using Restucode.Constants;
namespace Restucode.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class AccountController(IJwtTokenService jwtTokenService,
            UserManager<UserEntity> userManager, IMapper mapper, IImageService imageService) : ControllerBase
    {
        [HttpPost]
        public async Task<IActionResult> Login([FromBody] LoginModel model)
        {
            var user = await userManager.FindByEmailAsync(model.Email);
            if (user != null && await userManager.CheckPasswordAsync(user, model.Password))
            {
                var token = await jwtTokenService.CreateTokenAsync(user);
                return Ok(new { Token = token });
            }
            return Unauthorized("Invalid email or password");
        }

        [HttpPost]
        public async Task<IActionResult> Register([FromForm] RegisterModel model)
        {
            var entity = mapper.Map<UserEntity>(model);
            entity.UserName = model.Email;
            entity.Image = await imageService.SaveImageAsync(model.Image);
            var result = await userManager.CreateAsync(entity, model.Password);
            if (!result.Succeeded)
            {
                await userManager.AddToRoleAsync(entity, Roles.User);
                Console.WriteLine("Error Create User {0}", model.Email);
                return Unauthorized("User Creation Error {0}");
            }
            
            // Login after Register
            var user = await userManager.FindByEmailAsync(model.Email);
            if (user != null && await userManager.CheckPasswordAsync(user, model.Password))
            {
                var token = await jwtTokenService.CreateTokenAsync(user);
                return Ok(new { Token = token });
            }
            return Unauthorized("Something Went Wrong... try Logging in again.");
        }

    }
}
