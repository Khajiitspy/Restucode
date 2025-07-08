using AutoMapper;
using AutoMapper.QueryableExtensions;
using Core.Interface;
using Core.Models.AdminUser;
using Core.Models.General;
using Core.Models.Seeder;
using Core.Constants;
using Domain;
using Domain.Entities.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Diagnostics;
using Bogus;
using static Bogus.DataSets.Name;

namespace Core.Services;

public class UserService(UserManager<UserEntity> userManager,
    IMapper mapper,
    IImageService imageService,
    RoleManager<RoleEntity> roleManager,
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
                // user.LoginTypes.Add(login.LoginProvider);
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
                   // adminUser.LoginTypes.Add("Password");
               }
           }
       });

        return users;
    }

    public async Task<PagedResult<AdminUserItemModel>> GetFilteredUsersAsync(AdminUserFilterModel filter)
    {
        var query = userManager.Users.AsQueryable();

        if (!string.IsNullOrWhiteSpace(filter.FullName))
        {
            string nameFilter = filter.FullName.Trim().ToLower().Normalize();
            query = query.Where(u =>
                (u.FirstName + " " + u.LastName).ToLower().Contains(nameFilter) ||
                u.FirstName.ToLower().Contains(nameFilter) ||
                u.LastName.ToLower().Contains(nameFilter));
        }

        // if (filter.Roles != null && filter.Roles.Any())
        // {
        //     var validRoles = filter.Roles.Where(role => role != null);

        //     if (validRoles != null && validRoles.Count() > 0)
        //     {
        //         var usersInRole = (await Task.WhenAll(
        //             filter.Roles.Select(role => userManager.GetUsersInRoleAsync(role))
        //         )).SelectMany(u => u).ToList();

        //         var userIds = usersInRole.Select(u => u.Id).ToHashSet();

        //         query = query.Where(u => userIds.Contains(u.Id));
        //     }
        // }

        if (filter.Roles != null && filter.Roles.Any() && !filter.Roles.All(string.IsNullOrWhiteSpace))
        {
            var roleSet = filter.Roles.ToHashSet();
            query = query.Where(user =>
                user.UserRoles
                    .Select(ur => ur.Role.Name)
                    .Intersect(roleSet)
                    .Count() == roleSet.Count
            );
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

        // var users = await query
        //     .Skip((filter.Page - 1) * filter.PageSize)
        //     .Take(filter.PageSize)
        //     .ToListAsync();

        var users = await query
            .OrderBy(u => u.Id)
            .Skip((filter.Page - 1) * filter.PageSize)
            .Take(filter.PageSize)
            .ProjectTo<AdminUserItemModel>(mapper.ConfigurationProvider)
            .ToListAsync();

        // var result = users.Select(u => new AdminUserItemModel
        // {
        //     Id = u.Id,
        //     FullName = $"{u.FirstName} {u.LastName}",
        //     Email = u.Email!,
        //     Image = u.Image ?? "",
        //     LoginTypes = new List<string>(),
        //     Roles = new List<string>(),
        //     DateCreated = u.DateCreated
        // }).ToList();

        // var logins = await context.UserLogins.ToListAsync();
        // foreach (var login in logins)
        // {
        //     var target = result.FirstOrDefault(x => x.Id == login.UserId);
        //     if (target != null && !target.LoginTypes.Contains(login.LoginProvider))
        //     {
        //         target.LoginTypes.Add(login.LoginProvider);
        //     }
        // }

        // var identityUsers = await userManager.Users.AsNoTracking().ToListAsync();

        // foreach (var identityUser in identityUsers)
        // {
        //     var adminUser = result.FirstOrDefault(u => u.Id == identityUser.Id);
        //     if (adminUser != null)
        //     {
        //         var roles = await userManager.GetRolesAsync(identityUser);
        //         adminUser.Roles = roles.ToList();

        //         if (!string.IsNullOrEmpty(identityUser.PasswordHash))
        //         {
        //             adminUser.LoginTypes.Add("Password");
        //         }
        //     }
        // }

        return new PagedResult<AdminUserItemModel>
        {
            Items = users,
            TotalItems = total,
            Page = filter.Page,
            PageSize = filter.PageSize
        };
    }

    public async Task<string> SeedAsync(SeedItemsModel model)
    {
        Stopwatch stopWatch = new Stopwatch();
        stopWatch.Start();
        var fakeUsers = new Faker<SeederUserModel>("uk")
            .RuleFor(u => u.Gender, f => f.PickRandom<Gender>())
           //Pick some fruit from a basket
           .RuleFor(u => u.FirstName, (f, u) => f.Name.FirstName(u.Gender))
           .RuleFor(u => u.LastName, (f, u) => f.Name.LastName(u.Gender))
           .RuleFor(u => u.Email, (f, u) => f.Internet.Email(u.FirstName, u.LastName))
           .RuleFor(u => u.Password, (f, u) => f.Internet.Password(8))
           .RuleFor(u => u.Roles, f => new List<string>() { f.PickRandom(Constants.Roles.AllRoles) })
           .RuleFor(u => u.Image, f => "https://thispersondoesnotexist.com");
            
        var genUsers = fakeUsers.Generate(model.Count);

        try
        {
            foreach (var user in genUsers)
            {
                var entity = mapper.Map<UserEntity>(user);
                entity.UserName = user.Email;
                entity.Image = await imageService.SaveImageFromUrlAsync(user.Image);
                var result = await userManager.CreateAsync(entity, user.Password);
                if (!result.Succeeded)
                {
                    Console.WriteLine("Error Create User {0}", user.Email);
                    continue;
                }
                foreach (var role in user.Roles)
                {
                    if (await roleManager.RoleExistsAsync(role))
                    {
                        await userManager.AddToRoleAsync(entity, role);
                    }
                    else
                    {
                        Console.WriteLine("Not Found Role {0}", role);
                    }
                }
            }

        }
        catch (Exception ex)
        {
            Console.WriteLine("Error Json Parse Data {0}", ex.Message);
        }

        stopWatch.Stop();
        // Get the elapsed time as a TimeSpan value.
        TimeSpan ts = stopWatch.Elapsed;

        // Format and display the TimeSpan value.
        string elapsedTime = String.Format("{0:00}:{1:00}:{2:00}.{3:00}",
            ts.Hours, ts.Minutes, ts.Seconds,
            ts.Milliseconds / 10);

        return elapsedTime;
    }

    public async Task EditAsync(UserEditModel model)
    {
        var user = await userManager.Users
            .Include(u => u.UserRoles!)
            .FirstOrDefaultAsync(u => u.Id == model.Id);

        if (user == null)
            throw new Exception("User not found");

        user.FirstName = model.FirstName.Trim();
        user.LastName = model.LastName.Trim();

        if (model.Image != null)
        {
            if (!string.IsNullOrEmpty(user.Image))
            {
                imageService.DeleteImageAsync(user.Image);
            }

            user.Image = await imageService.SaveImageAsync(model.Image);
        }

        var currentRoles = await userManager.GetRolesAsync(user);
        var rolesToAdd = model.Roles.Except(currentRoles).ToList();
        var rolesToRemove = currentRoles.Except(model.Roles).ToList();

        if (rolesToAdd.Any())
            await userManager.AddToRolesAsync(user, rolesToAdd);

        if (rolesToRemove.Any())
            await userManager.RemoveFromRolesAsync(user, rolesToRemove);

        await userManager.UpdateAsync(user);
    }

    public async Task<UserEditViewModel> GetByIdAsync(long id)
    {
        var user = await userManager.Users
            .Include(u => u.UserRoles!)
            .FirstOrDefaultAsync(u => u.Id == id);

        if (user == null)
            throw new Exception("User not found");

        var roles = await userManager.GetRolesAsync(user);

        return new UserEditViewModel
        {
            Id = user.Id,
            FirstName = user.FirstName ?? string.Empty,
            LastName = user.LastName ?? string.Empty,
            ViewImage = user.Image,
            Roles = roles.ToList()
        };
    }

    public async Task<List<string>> GetAllRoles(){
        return await roleManager.Roles
            .Select(r => r.Name!)
            .ToListAsync();
    }
 }
