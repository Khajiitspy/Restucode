namespace Core.Models.AdminUser;

public class UserEditViewModel
{
    public long Id { get; set; }
    public string FirstName { get; set; } = default!;
    public string LastName { get; set; } = default!;
    public string? ViewImage { get; set; }
    public List<string> Roles { get; set; } = new();
}
