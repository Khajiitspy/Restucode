using Microsoft.AspNetCore.Http;

namespace Core.Models.AdminUser;

public class UserEditModel
{
    public long Id { get; set; }
    public string FirstName { get; set; } = default!;
    public string LastName { get; set; } = default!;
    public IFormFile? Image { get; set; }
    public List<string> Roles { get; set; } = new();
}
