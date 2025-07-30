using Microsoft.AspNetCore.Http;

namespace Core.Models.Account;

public class FullNameModel
{
    public string FirstName { get; set; } = default!;
    public string LastName { get; set; } = default!;
}
