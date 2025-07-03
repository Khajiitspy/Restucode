using AutoMapper;
using AutoMapper.QueryableExtensions;
using Core.Interface;
using Core.Models.AdminUser;
using Core.Models.General;
using Domain;
using Domain.Entities.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Core.Services;

public class UserService(UserManager<UserEntity> userManager,
    IMapper mapper,
    RestucodeDBContext context) : IUserService
{
    public async Task<List<AdminUserItemModel>> GetAllUsersAsync()
    {
        var users = await userManager.Users
            .ProjectTo<AdminUserItemModel>(mapper.ConfigurationProvider)
            .ToListAsync();

        await context.UserLogins.ForEachAsync(login =>
        {
            var user = users.FirstOrDefault(u => u.Id == login.UserId);
            if (user != null)
            {
                user.LoginTypes.Add(login.LoginProvider);
            }
        });

        await context.Users
       .ForEachAsync(user =>
       {
           var adminUser = users.FirstOrDefault(u => u.Id == user.Id);
           if (adminUser != null)
           {
               if (!string.IsNullOrEmpty(user.PasswordHash))
               {
                   adminUser.LoginTypes.Add("Password");
               }
           }
       });

        return users;
    }

    public async Task<PagedResult<AdminUserItemModel>> GetFilteredUsersAsync(AdminUserFilterModel filter)
    {
        var query = userManager.Users
            .Include(u => u.UserRoles)
                .ThenInclude(ur => ur.Role)
            .AsQueryable();

        var logs = new List<string>();

        logs.Add($"User count before filtering: {await userManager.Users.CountAsync()}");

        if (!string.IsNullOrWhiteSpace(filter.FullName))
        {
            query = query.Where(u =>
                (u.FirstName + " " + u.LastName).Contains(filter.FullName));
        }

        var allRoles = await query.SelectMany(u => u.UserRoles.Select(ur => ur.Role.Name)).Distinct().ToListAsync();
        
        if (!string.IsNullOrWhiteSpace(filter.Role))
        {
            var roleLower = filter.Role.ToLower();
            query = query.Where(u =>
                u.UserRoles.Any(ur => ur.Role.Name.ToLower() == roleLower)); // ✅ avoids LIKE problems
        }

        if (filter.RegisteredFrom.HasValue)
        {
            var from = DateTime.SpecifyKind(filter.RegisteredFrom.Value, DateTimeKind.Utc);
            query = query.Where(u => u.DateCreated >= from);
        }

        if (filter.RegisteredTo.HasValue)
        {
            var to = DateTime.SpecifyKind(filter.RegisteredTo.Value, DateTimeKind.Utc);
            query = query.Where(u => u.DateCreated <= to);
        }

        var total = await query.CountAsync();

        var users = await query
            .Skip((filter.Page - 1) * filter.PageSize)
            .Take(filter.PageSize)
            .ToListAsync();

        var result = users.Select(u => new AdminUserItemModel
        {
            Id = u.Id,
            FullName = $"{u.FirstName} {u.LastName}",
            Email = u.Email!,
            Image = u.Image ?? "",
            LoginTypes = new List<string>()
        }).ToList();

        var logins = await context.UserLogins.ToListAsync();
        foreach (var login in logins)
        {
            var target = result.FirstOrDefault(x => x.Id == login.UserId);
            if (target != null && !target.LoginTypes.Contains(login.LoginProvider))
            {
                target.LoginTypes.Add(login.LoginProvider);
            }
        }

        foreach (var user in users)
        {
            var adminUser = result.FirstOrDefault(x => x.Id == user.Id);
            if (adminUser != null && !string.IsNullOrEmpty(user.PasswordHash))
            {
                adminUser.LoginTypes.Add("Password");
            }
        }

        return new PagedResult<AdminUserItemModel>
        {
            Items = result,
            TotalItems = total,
            Page = filter.Page,
            PageSize = filter.PageSize
        };
    }
}
