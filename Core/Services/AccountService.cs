using System.Net.Http.Headers;
using System.Net.Mail;
using System.Text.Json;
using System;
using System.Diagnostics;
using AutoMapper;
using Core.Interface;
using Core.Models.Account;
using Core.SMTP;
using Domain.Entities.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace Core.Services;

public class AccountService(IJwtTokenService tokenService,
    UserManager<UserEntity> userManager,
    IMapper mapper,
    IConfiguration configuration,
    IImageService imageService,
    ISmtpService smtpService,
    IAuthService authservice
    ) : IAccountService
{

    public async Task<string> LoginByGoogle(string token)
    {
        using var httpClient = new HttpClient();

        httpClient.DefaultRequestHeaders.Authorization =
            new AuthenticationHeaderValue("Bearer", token);

        //configuration
        string userInfo = configuration["GoogleUserInfo"] ?? "https://www.googleapis.com/oauth2/v2/userinfo";
        var response = await httpClient.GetAsync(userInfo);

        if (!response.IsSuccessStatusCode)
            return null;

        var json = await response.Content.ReadAsStringAsync();

        var googleUser = JsonSerializer.Deserialize<GoogleAccountModel>(json);

        var existingUser = await userManager.FindByEmailAsync(googleUser!.Email);
        if (existingUser != null)
        {
            var userLoginGoogle = await userManager.FindByLoginAsync("Google", googleUser.GogoleId);

            if (userLoginGoogle == null)
            {
                await userManager.AddLoginAsync(existingUser, new UserLoginInfo("Google", googleUser.GogoleId, "Google"));
            }
            var jwtToken = await tokenService.CreateTokenAsync(existingUser);
            return jwtToken;
        }
        else
        {
            var user = mapper.Map<UserEntity>(googleUser);

            if (!String.IsNullOrEmpty(googleUser.Picture))
            {
                user.Image = await imageService.SaveImageFromUrlAsync(googleUser.Picture);
            }

            var result = await userManager.CreateAsync(user);
            if (result.Succeeded)
            {

                result = await userManager.AddLoginAsync(user, new UserLoginInfo(
                    loginProvider: "Google",
                    providerKey: googleUser.GogoleId,
                    displayName: "Google"
                ));

                await userManager.AddToRoleAsync(user, "User");
                var jwtToken = await tokenService.CreateTokenAsync(user);
                return jwtToken;
            }
        }

        return string.Empty;
    }
    public async Task<bool> ForgotPasswordAsync(ForgotPasswordModel model)
    {
        var user = await userManager.FindByEmailAsync(model.Email);

        if (user == null)
        {
            return false;
        }

        string token = await userManager.GeneratePasswordResetTokenAsync(user);
        var resetLink = $"{configuration["ClientUrl"]}/reset-password?token={Uri.EscapeDataString(token)}&email={Uri.EscapeDataString(model.Email)}";

        var emailModel = new EmailMessage
        {
            To = model.Email,
            Subject = "Password Reset",
            Body = $"<p>Click the link below to reset your password:</p><a href='{resetLink}'>Reset Password</a>"
        };

        var result = await smtpService.SendEmailAsync(emailModel);

        return result;
    }

    public async Task<bool> ValidateResetTokenAsync(ValidateTokenModel model)
    {
        var user = await userManager.FindByEmailAsync(model.Email);

        return await userManager.VerifyUserTokenAsync(
            user,
            TokenOptions.DefaultProvider,
            "ResetPassword",
            model.Token);
    }

    public async Task ResetPasswordAsync(ResetPasswordModel model)
    {
        var user = await userManager.FindByEmailAsync(model.Email);

        if (user != null)
            await userManager.ResetPasswordAsync(user, model.Token, model.Password);
    }

    public async Task<UserEntity> GetUser(){
        var userId = await authservice.GetuserId();
        var user = await userManager.Users
            .Include(u => u.UserRoles!)
            .FirstOrDefaultAsync(u => u.Id == userId);

        if (user == null)
            throw new Exception("User not found");

        return user;
    }

    public async Task<FullNameModel> GetFullName(){
        var userId = await authservice.GetuserId();
        var user = await userManager.Users
            .Include(u => u.UserRoles!)
            .FirstOrDefaultAsync(u => u.Id == userId);

        if (user == null)
            throw new Exception("User not found");

        return new FullNameModel {
            FirstName = user.FirstName,
            LastName = user.LastName
        };
    }

    public async Task EditProfile(ProfileEditModel model)
    {
        var userId = await authservice.GetuserId();
        var user = await userManager.Users
            .Include(u => u.UserRoles!)
            .FirstOrDefaultAsync(u => u.Id == userId);

        if (user == null)
            throw new Exception("User not found");

        user.FirstName = model.FirstName.Trim();
        user.LastName = model.LastName.Trim();
        user.Email = model.Email.Trim();

        if (model.Image != null)
        {
            if (!string.IsNullOrEmpty(user.Image))
            {
                imageService.DeleteImageAsync(user.Image);
            }

            user.Image = await imageService.SaveImageAsync(model.Image);
        }

        await userManager.UpdateAsync(user);
    }
}
