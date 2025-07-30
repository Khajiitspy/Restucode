using Microsoft.AspNetCore.Http;

namespace Core.Models.Account;

public class ProfileEditModel
{
    public string FirstName { get; set; } = default!;
    public string LastName { get; set; } = default!;
    public string Email {get; set; } = default!;
    public IFormFile? Image { get; set; }
}
