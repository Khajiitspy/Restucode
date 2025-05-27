using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Restucode.Data.Entities.Identity;
using Restucode.Data.Entities;

namespace Restucode.Data;

public class RestucodeDBContext : IdentityDbContext<UserEntity, RoleEntity, long>
{
    public RestucodeDBContext(DbContextOptions<RestucodeDBContext> opt) : base(opt) { }


    public DbSet<CategoryEntity> Categories { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        builder.Entity<UserRoleEntity>(ur =>
        {
            ur.HasOne(ur => ur.Role)
                .WithMany(r => r.UserRoles)
                .HasForeignKey(r => r.RoleId)
                .IsRequired();

            ur.HasOne(ur => ur.User)
                .WithMany(r => r.UserRoles)
                .HasForeignKey(u => u.UserId)
                .IsRequired();
        });
    }
}
