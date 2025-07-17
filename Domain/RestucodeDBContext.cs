using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Domain.Entities.Identity;
using Domain.Entities.Delivery;
using Domain.Entities;
using System.Reflection.Emit;

namespace Domain;

public class RestucodeDBContext : IdentityDbContext<UserEntity, RoleEntity, long,
        IdentityUserClaim<long>, UserRoleEntity, UserLoginEntity,
        IdentityRoleClaim<long>, IdentityUserToken<long>>
{
    public RestucodeDBContext(DbContextOptions<RestucodeDBContext> opt) : base(opt) { }


    public DbSet<CategoryEntity> Categories { get; set; }
    public DbSet<IngredientEntity> Ingredients { get; set; }
    public DbSet<ProductSizeEntity> ProductSizes { get; set; }
    public DbSet<ProductEntity> Products { get; set; }
    public DbSet<ProductVariantEntity> ProductVariants { get; set; }
    public DbSet<ProductIngredientEntity> ProductIngredients { get; set; }
    public DbSet<ProductImageEntity> ProductImages { get; set; }
    public DbSet<CartEntity> Carts { get; set; }
    public DbSet<OrderStatusEntity> OrderStatuses { get; set; }
    public DbSet<OrderEntity> Orders { get; set; }
    public DbSet<OrderItemEntity> OrderItems { get; set; }
    public DbSet<CityEntity> Cities { get; set; }
    public DbSet<PostDepartmentEntity> PostDepartments { get; set; }
    public DbSet<PaymentTypeEntity> PaymentTypes { get; set; }
    public DbSet<DeliveryInfoEntity> DeliveryInfos { get; set; }

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

        builder.Entity<ProductVariantEntity>()
            .HasMany(pv => pv.ProductImages)
            .WithOne(pi => pi.ProductVariant)
            .HasForeignKey(pi => pi.ProductVariantId);

        builder.Entity<UserLoginEntity>(b =>
        {
            b.HasOne(l => l.User)
                .WithMany(u => u.UserLogins)
                .HasForeignKey(l => l.UserId)
                .IsRequired();
        });

        builder.Entity<ProductIngredientEntity>()
            .HasKey(pi => new { pi.ProductVariantId, pi.IngredientId });
        
        builder.Entity<CartEntity>()
            .HasKey(pi => new { pi.ProductVariantId, pi.UserId });
    }
}
