namespace Core.Models.AdminUser;

public class AdminUserFilterModel
{
    public string? FullName { get; set; }
    public string? Role { get; set; }
    public DateTime? RegisteredFrom { get; set; }
    public DateTime? RegisteredTo { get; set; }
    public int Page { get; set; } = 1;
    public int PageSize { get; set; } = 10;
}
