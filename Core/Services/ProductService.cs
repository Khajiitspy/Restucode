using AutoMapper;
using Core.Interface;
using Core.Models.Category;
using Core.Models.General;
using Core.Models.Product;
using Domain;
using Domain.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Services
{
    public class ProductService(RestucodeDBContext context, IMapper mapper, IImageService imageService) : IProductService
    {
        public async Task<PagedResult<ProductItemViewModel>> List(string? search, int page = 1, int pageSize = 5)
        {
            var query = context.Products
                .Include(p => p.ProductVariants)
                    .ThenInclude(v => v.ProductImages)
                .Include(p => p.ProductVariants)
                    .ThenInclude(v => v.ProductSize)
                .Include(p => p.ProductVariants)
                    .ThenInclude(v => v.Category)
                .Include(p => p.ProductVariants)
                    .ThenInclude(v => v.ProductIngredients)
                        .ThenInclude(pi => pi.Ingredient)
                .AsQueryable();


            if (!string.IsNullOrEmpty(search))
                query = query.Where(p => p.ProductVariants.Where(v => v.Name.Contains(search)).Count() >= 1);

            var totalItems = await query.CountAsync();

            var items = await mapper.ProjectTo<ProductItemViewModel>(query)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return new PagedResult<ProductItemViewModel>
            {
                Items = items,
                TotalItems = totalItems,
                Page = page,
                PageSize = pageSize
            };
        }

        public async Task<ProductDetailsViewModel> Details(long id)
        {
            var product = context.Products
                .Include(p => p.ProductVariants)
                    .ThenInclude(v => v.ProductImages)
                .Include(p => p.ProductVariants)
                    .ThenInclude(v => v.ProductSize)
                .Include(p => p.ProductVariants)
                    .ThenInclude(v => v.Category)
                .Include(p => p.ProductVariants)
                    .ThenInclude(v => v.ProductIngredients)
                        .ThenInclude(pi => pi.Ingredient)
                .AsQueryable();

            var model = mapper.Map<ProductDetailsViewModel>(product.Where(p => p.Id == id).First());
            return model;
        }

        public async Task<long> CreateProduct(ProductCreateModel model)
        {
            var existing = await context.Products
                .Include(p => p.ProductVariants)
                .FirstOrDefaultAsync(p => p.Slug == model.Slug);

            var categoryId = context.Categories.FirstOrDefault(c => c.Name == model.Category)?.Id ?? 0;
            var sizeId = context.ProductSizes.FirstOrDefault(s => s.Name == model.Size)?.Id;

            var variant = new ProductVariantEntity
            {
                Name = model.Name,
                Price = model.Price,
                Weight = model.Weight,
                CategoryId = categoryId,
                ProductSizeId = sizeId
            };

            if (existing != null)
            {
                existing.ProductVariants.Add(variant);
            }
            else
            {
                existing = new ProductEntity
                {
                    Slug = model.Slug,
                    ProductVariants = new List<ProductVariantEntity> { variant }
                };
                context.Products.Add(existing);
            }

            await context.SaveChangesAsync();

            for (int i = 0; i < model.IngredientNames.Count; i++)
            {
                var name = model.IngredientNames[i];

                var ingredient = await context.Ingredients
                    .FirstOrDefaultAsync(i => i.Name == name);

                if (ingredient == null)
                {
                    var imageFile = model.IngredientImages[i];
                    var savedImage = await imageService.SaveImageAsync(imageFile);

                    ingredient = new IngredientEntity
                    {
                        Name = name,
                        Image = savedImage
                    };

                    context.Ingredients.Add(ingredient);
                    await context.SaveChangesAsync();
                }

                context.ProductIngredients.Add(new ProductIngredientEntity
                {
                    ProductVariantId = variant.Id,
                    IngredientId = ingredient.Id
                });
            }

            short priority = 1;
            foreach (var image in model.Images)
            {
                var savedImage = await imageService.SaveImageAsync(image);

                context.ProductImages.Add(new ProductImageEntity
                {
                    ProductVariantId = variant.Id,
                    Name = savedImage,
                    Priority = priority++
                });
            }

            await context.SaveChangesAsync();

            return existing.Id;
        }

    }
}
