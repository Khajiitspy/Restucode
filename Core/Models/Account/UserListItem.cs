namespace Core.Models.Account;

public class UserListItemViewModel
{
    public long Id { get; set; }
    public string Email { get; set; } = string.Empty;
    public string? Username { get; set; }
    public string? Image { get; set; }
    public string? LoginProvider { get; set; } // null = regular login
}
