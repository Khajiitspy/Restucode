using Google.Apis.Auth;

namespace Core.Interface;

public interface IGoogleService
{
    Task<GoogleJsonWebSignature.Payload?> VerifyTokenAsync(string token);
}