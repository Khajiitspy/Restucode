using System.Text.Json;
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Core.Constants;
using Domain;
using Domain.Entities;
using Domain.Entities.Identity;
using Core.Interface;
using Core.Models.Seeder;
using Core.Services;

namespace Restucode;

public static class DbSeeder
{
    public static async Task SeedData(this WebApplication webApplication)
    {
        using var scope = webApplication.Services.CreateScope();
        //Цей об'єкт буде верта посилання на конткетс, який зараєстрвоано в Progran.cs
        var context = scope.ServiceProvider.GetRequiredService<RestucodeDBContext>();
        var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<RoleEntity>>();
        var userManager = scope.ServiceProvider.GetRequiredService<UserManager<UserEntity>>();
        var mapper = scope.ServiceProvider.GetRequiredService<IMapper>();

        context.Database.Migrate();

        if (!context.Categories.Any())
        {
            var imageService = scope.ServiceProvider.GetRequiredService<IImageService>();
            var jsonFile = Path.Combine(Directory.GetCurrentDirectory(), "Helpers", "JsonData", "Categories.json");
            if (File.Exists(jsonFile))
            {
                var jsonData = await File.ReadAllTextAsync(jsonFile);
                try
                {
                    var categories = JsonSerializer.Deserialize<List<SeederCategoryModel>>(jsonData);
                    var entityItems = mapper.Map<List<CategoryEntity>>(categories);
                    foreach (var entity in entityItems)
                    {
                        entity.Image =
                            await imageService.SaveImageFromUrlAsync(entity.Image);
                    }

                    await context.AddRangeAsync(entityItems);
                    await context.SaveChangesAsync();

                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error Json Parse Data {0}", ex.Message);
                }
            }
            else
            {
                Console.WriteLine("Not Found File Categories.json");
            }
        }

        if(!context.Roles.Any())
        {
            foreach(var roleName in Roles.AllRoles)
            {
                var result = await roleManager.CreateAsync(new(roleName));
                if (!result.Succeeded)
                {
                    Console.WriteLine("Error Create Role {0}", roleName);
                }
            }
            await context.SaveChangesAsync();
        }

        if (!context.Users.Any())
        {
            var imageService = scope.ServiceProvider.GetRequiredService<IImageService>();
            var jsonFile = Path.Combine(Directory.GetCurrentDirectory(), "Helpers", "JsonData", "Users.json");
            if (File.Exists(jsonFile))
            {
                var jsonData = await File.ReadAllTextAsync(jsonFile);
                try
                {
                    var users = JsonSerializer.Deserialize<List<SeederUserModel>>(jsonData);
                    foreach (var user in users)
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
            }
            else
            {
                Console.WriteLine("Not Found File Users.json");
            }
        }

        if (!context.Ingredients.Any())
        {
            var imageService = scope.ServiceProvider.GetRequiredService<IImageService>();
            var jsonFile = Path.Combine(Directory.GetCurrentDirectory(), "Helpers", "JsonData", "Ingredients.json");
            if (File.Exists(jsonFile))
            {
                var jsonData = await File.ReadAllTextAsync(jsonFile);
                try
                {
                    var Ingredients = JsonSerializer.Deserialize<List<SeederIngredientModel>>(jsonData);
                    var entityItems = mapper.Map<List<IngredientEntity>>(Ingredients);
                    foreach (var entity in entityItems)
                    {
                        entity.Image =
                            await imageService.SaveImageFromUrlAsync(entity.Image);
                    }

                    await context.AddRangeAsync(entityItems);
                    await context.SaveChangesAsync();

                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error Json Parse Data {0}", ex.Message);
                }
            }
            else
            {
                Console.WriteLine("Not Found File Ingredients.json");
            }
        }

        if (!context.ProductSizes.Any())
        {
            var jsonFile = Path.Combine(Directory.GetCurrentDirectory(), "Helpers", "JsonData", "ProductSizes.json");
            if (File.Exists(jsonFile))
            {
                var jsonData = await File.ReadAllTextAsync(jsonFile);
                try
                {
                    var items = JsonSerializer.Deserialize<List<SeederProductSizeModel>>(jsonData);
                    var entityItems = mapper.Map<List<ProductSizeEntity>>(items);
                    await context.ProductSizes.AddRangeAsync(entityItems);
                    await context.SaveChangesAsync();

                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error Json Parse Data {0}", ex.Message);
                }
            }
            else
            {
                Console.WriteLine("Not Found File ProductSizes.json");
            }
        }

        if (!context.Products.Any())
        {
            var imageService = scope.ServiceProvider.GetRequiredService<IImageService>();

            var сaesar = new ProductEntity
            {
                ProductVariants = new List<ProductVariantEntity>()
                {
                    new ProductVariantEntity() {
                        Name = "Цезаре",
                        Price = 195,
                        Weight = 540,
                        CategoryId = 1,
                        ProductSizeId = 1
                    }
                },
                Slug = "caesar",
            };

            context.Products.Add(сaesar);
            await context.SaveChangesAsync();

            var variant = сaesar.ProductVariants.First();

            var ingredients = context.Ingredients.ToList();

            foreach (var ingredient in ingredients)
            {
                var productIngredient = new ProductIngredientEntity
                {
                    ProductVariantId = variant.Id,
                    IngredientId = ingredient.Id
                };
                context.ProductIngredients.Add(productIngredient);
            }
            await context.SaveChangesAsync();

            string[] images = {
                "https://matsuri.com.ua/img_files/gallery_commerce/products/big/commerce_products_images_51.webp?47d1e990583c9c67424d369f3414728e",
                "https://cdn.lifehacker.ru/wp-content/uploads/2022/03/11187_1522960128.7729_1646727034-1170x585.jpg",
            };

            int p = 0;
            foreach (var imageUrl in images)
            {
                var productImage = new ProductImageEntity
                {
                    ProductVariantId = variant.Id,
                    Name = await imageService.SaveImageFromUrlAsync(imageUrl),
                    Priority = (short)(++p)
                };
                context.ProductImages.Add(productImage);
            }
            await context.SaveChangesAsync();


        }
        
        if (!context.OrderStatuses.Any())
        {
            List<string> names = new List<string>() { 
                "Нове", "Очікує оплати", "Оплачено", 
                "В обробці", "Готується до відправки", 
                "Відправлено", "У дорозі", "Доставлено",    
                "Завершено", "Скасовано (вручну)", "Скасовано (автоматично)", 
                "Повернення", "В обробці повернення" };

            var orderStatuses = names.Select(name => new OrderStatusEntity { Name = name }).ToList();

            await context.OrderStatuses.AddRangeAsync(orderStatuses);
            await context.SaveChangesAsync();
        }
        
        
        if (!context.Orders.Any())
        {
            var jsonFile = Path.Combine(Directory.GetCurrentDirectory(), "Helpers", "JsonData", "Orders.json");
            if (File.Exists(jsonFile))
            {
                var jsonData = await File.ReadAllTextAsync(jsonFile);
                try
                {
                    var orders = JsonSerializer.Deserialize<List<SeederOrderModel>>(jsonData);
                    foreach (var order in orders!)
                    {
                        var user = await context.Users.FirstOrDefaultAsync(u => u.Email == order.userEmail);
                        var status = await context.OrderStatuses.FirstOrDefaultAsync(s => s.Id == order.status);

                        if (user == null || status == null)
                        {
                            Console.WriteLine($"Missing user or status for order: {order.userEmail}, {order.status}");
                            continue;
                        }

                        var orderEntity = new OrderEntity
                        {
                            UserId = user.Id,
                            OrderStatusId = status.Id,
                            OrderItems = new List<OrderItemEntity>()
                        };

                        foreach (var item in order.items)
                        {
                            var variant = await context.ProductVariants
                                .FirstOrDefaultAsync(pv => pv.Id == item.productVariantId);

                            if (variant == null)
                            {
                                Console.WriteLine($"Missing product variant: {item.productVariantId}");
                                continue;
                            }

                            orderEntity.OrderItems.Add(new OrderItemEntity
                            {
                                ProductVariantId = variant.Id,
                                Count = item.count,
                                PriceBuy = item.priceBuy
                            });
                        }

                        await context.Orders.AddAsync(orderEntity);
                    }

                    await context.SaveChangesAsync();
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error parsing Orders.json: {0}", ex.Message);
                }
            }
            else
            {
                Console.WriteLine("Not Found File Orders.json");
            }
        }
        
    }
}
