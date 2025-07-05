using Core.Models.AdminUser;
using Core.Models.General;
using Core.Models.Seeder;

namespace Core.Interface;

public interface IUserService
{
    Task<List<AdminUserItemModel>> GetAllUsersAsync();
    Task<PagedResult<AdminUserItemModel>> GetFilteredUsersAsync(AdminUserFilterModel filter);
    Task<string> SeedAsync(SeedItemsModel model);
    Task EditAsync(UserEditModel request);
    Task<UserEditViewModel> GetByIdAsync(long id);
    Task<List<string>> GetAllRoles();
}
