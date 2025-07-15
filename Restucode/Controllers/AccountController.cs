using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Domain.Entities.Identity;
using Core.Interface;
using Core.Models.Account;
using System.Security.Claims;
using Core.Constants;
using Core.SMTP;
using Microsoft.EntityFrameworkCore;

namespace Restucode.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class AccountController(IJwtTokenService jwtTokenService,
            UserManager<UserEntity> userManager, IMapper mapper, IImageService imageService, IGoogleService googleService, IAccountService accountService, ISmtpService smtpService) : ControllerBase
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

        // [HttpPost]
        // public async Task<IActionResult> Register([FromForm] RegisterModel model)
        // {
        //     var entity = mapper.Map<UserEntity>(model);
        //     entity.UserName = model.Email;
        //     entity.Image = await imageService.SaveImageAsync(model.Image);
        //     var result = await userManager.CreateAsync(entity, model.Password);
        //     if (!result.Succeeded)
        //     {
        //         await userManager.AddToRoleAsync(entity, Roles.User);
        //         Console.WriteLine("Error Create User {0}", model.Email);
        //         return Unauthorized("User Creation Error {0}");
        //     }
            
        //     // Login after Register
        //     var user = await userManager.FindByEmailAsync(model.Email);
        //     if (user != null && await userManager.CheckPasswordAsync(user, model.Password))
        //     {
        //         var token = await jwtTokenService.CreateTokenAsync(user);
        //         return Ok(new { Token = token });
        //     }
        //     return Unauthorized("Something Went Wrong... try Logging in again.");
        // }

        [HttpPost]
        public async Task<IActionResult> Register([FromForm] RegisterModel model)
        {
            var user = mapper.Map<UserEntity>(model);

            user.Image = await imageService.SaveImageAsync(model.Image!);

            var result = await userManager.CreateAsync(user, model.Password);
            if (result.Succeeded)
            {
                await userManager.AddToRoleAsync(user, Roles.User);
                var token = await jwtTokenService.CreateTokenAsync(user);
                return Ok(new
                {
                    Token = token
                });
            }
            else
            {
                return BadRequest(new
                {
                    status = 400,
                    isValid = false,
                    errors = "Registration failed"
                });
            }

        }
        
        // [HttpPost]
        // public async Task<IActionResult> GoogleLogin([FromBody] GoogleToken model)
        // {
        //     var payload = await googleService.VerifyTokenAsync(model.Token);
        //     if (payload == null)
        //         return Unauthorized();

        //     var user = await userManager.FindByEmailAsync(payload.Email);
        //     if (user != null)
        //     {
        //         var token = await jwtTokenService.CreateTokenAsync(user);
        //         return Ok(new { Token = token });
        //     }
        //     return Unauthorized("This email is not registered");
        // }
        
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
        [HttpPost]
        public async Task<IActionResult> GoogleLogin2([FromBody] GoogleToken model)
        {
            string result = await accountService.LoginByGoogle(model.Token);
            if (string.IsNullOrEmpty(result))
            {
                return BadRequest(new
                {
                    Status = 400,
                    IsValid = false,
                    Errors = new { Email = "Помилка реєстрації" }
                });
            }
            return Ok(new
            {
                Token = result
            });
        }
        [HttpPost]
        public async Task<IActionResult> ForgotPassword([FromBody] ForgotPasswordModel model)
        {
            bool res = await accountService.ForgotPasswordAsync(model);
            if (res)
                return Ok();
            else
                return BadRequest(new
                {
                    Status = 400,
                    IsValid = false,
                    Errors = new { Email = "Користувача з такою поштою не існує" }
                });
        }

        [HttpGet]
        public async Task<IActionResult> ValidateResetToken([FromQuery] ValidateTokenModel model)
        {
            bool res = await accountService.ValidateResetTokenAsync(model);
            return Ok(new { IsValid = res });
        }

        [HttpPost]
        public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordModel model)
        {
            await accountService.ResetPasswordAsync(model);
            return Ok();
        }
        
        // [HttpGet]
        // [Authorize(Roles = Roles.Admin)]
        // public async Task<IEnumerable<UserListItemViewModel>> GetUsers()
        // {
        //     var query = userManager.Users.Include(u => u.Logins).AsQueryable();

        //     return await mapper.ProjectTo<UserListItemViewModel>(query)
        //         .ToListAsync();
        // }

    }
}
