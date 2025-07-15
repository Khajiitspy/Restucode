using Core.Models.Account;

namespace Core.Interface;

public interface IAccountService
{
    public Task<string> LoginByGoogle(string token);
    public Task<bool> ForgotPasswordAsync(ForgotPasswordModel model);
    public Task<bool> ValidateResetTokenAsync(ValidateTokenModel model);
    public Task ResetPasswordAsync(ResetPasswordModel model);
}
