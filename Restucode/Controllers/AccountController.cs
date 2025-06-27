using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Domain.Entities.Identity;
using Core.Interface;
using Core.Models.Account;
using System.Security.Claims;
using Restucode.Constants;
namespace Restucode.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class AccountController(IJwtTokenService jwtTokenService,
            UserManager<UserEntity> userManager, IMapper mapper, IImageService imageService, IGoogleService googleService) : ControllerBase
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
        
        [HttpPost]
        public async Task<IActionResult> GoogleLogin([FromBody] GoogleToken model)
        {
            var payload = await googleService.VerifyTokenAsync(model.Token);
            if (payload == null)
                return Unauthorized();

            var user = await userManager.FindByEmailAsync(payload.Email);
            if (user != null)
            {
                var token = await jwtTokenService.CreateTokenAsync(user);
                return Ok(new { Token = token });
            }
            return Unauthorized("This email is not registered");
        }
        
        [HttpPost]
        public async Task<IActionResult> GoogleRegister([FromBody] GoogleToken model)
        {
            var payload = await googleService.VerifyTokenAsync(model.Token);
            if (payload == null)
                return Unauthorized(new { error = "Invalid Google token" });

            var existingUser = await userManager.FindByEmailAsync(payload.Email);
            if (existingUser != null)
                return BadRequest(new { error = "User already exists" });

            var user = new UserEntity
            {
                Email = payload.Email,
                UserName = payload.Email,
                FirstName = payload.GivenName,
                LastName = payload.FamilyName,
                Image = await imageService.SaveImageFromUrlAsync(payload.Picture)
            };

            var randomPassword = Guid.NewGuid().ToString() + "!Aa1";
            var result = await userManager.CreateAsync(user, randomPassword);

            if (!result.Succeeded)
                return BadRequest(new { error = "Failed to create user" });

            await userManager.AddToRoleAsync(user, Roles.User);

            var jwt = await jwtTokenService.CreateTokenAsync(user);
            return Ok(new { Token = jwt });
        }
    }
}
