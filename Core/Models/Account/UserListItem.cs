namespace Core.Models.Account;

public class UserListItemViewModel
{
    public long Id { get; set; }
    public string Email { get; set; } = string.Empty;
    public string? Username { get; set; }
    public string? Image { get; set; }
    public List<string> Roles {get; set;} = new();
    public List<string> LoginTypes {get; set;} = new();
}
