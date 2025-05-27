using Restucode.Data.Entities.Identity;

namespace Restucode.Interface;

public interface IJwtTokenService
{
    Task<string> CreateTokenAsync(UserEntity user); 
}
