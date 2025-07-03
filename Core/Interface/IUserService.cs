using Core.Models.AdminUser;
using Core.Models.General;

namespace Core.Interface;

public interface IUserService
{
    Task<List<AdminUserItemModel>> GetAllUsersAsync();
    Task<PagedResult<AdminUserItemModel>> GetFilteredUsersAsync(AdminUserFilterModel filter);
}
