using Core.Interface;
using Google.Apis.Auth;
using Microsoft.Extensions.Configuration;

namespace Core.Services;

public class GoogleService(IConfiguration config) : IGoogleService
{
    public async Task<GoogleJsonWebSignature.Payload?> VerifyTokenAsync(string token)
    {
        try
        {
            var settings = new GoogleJsonWebSignature.ValidationSettings
            {
                Audience = new[] { config["Google:ClientId"] }
            };

            var payload = await GoogleJsonWebSignature.ValidateAsync(token, settings);
            return payload;
        }
        catch (Exception)
        {
            return null; 
        }
    }
}
