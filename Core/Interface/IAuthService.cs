namespace Core.Interface;

public interface IAuthService
{
    public Task<long> GetuserId();
    public Task<string> GetUserEmail();
}
