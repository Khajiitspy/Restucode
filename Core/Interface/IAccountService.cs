using Core.Models.Account;
using Domain.Entities.Identity;

namespace Core.Interface;

public interface IAccountService
{
    public Task<string> LoginByGoogle(string token);
    public Task<bool> ForgotPasswordAsync(ForgotPasswordModel model);
    public Task<bool> ValidateResetTokenAsync(ValidateTokenModel model);
    public Task ResetPasswordAsync(ResetPasswordModel model);
    public Task EditProfile(ProfileEditModel model);
    public Task<FullNameModel> GetFullName();
    public Task<UserEntity> GetUser();
}
