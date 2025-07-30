using AutoMapper;
using AutoMapper.QueryableExtensions;
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
        public async Task<PagedResult<ProductItemViewModel>> List(ProductSearchModel filter)
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


            if (!string.IsNullOrEmpty(filter.Name))
                query = query.Where(p => p.ProductVariants.Where(v => v.Name.ToLower().Contains(filter.Name.ToLower())).Count() >= 1);

            if(filter.CategoryId != null){
                query = query.Where(p => p.ProductVariants.Where(v => v.CategoryId == filter.CategoryId).Count() >= 1);
            }

            var totalItems = await query.CountAsync();

            var items = await mapper.ProjectTo<ProductItemViewModel>(query)
                .Skip((filter.Page - 1) * filter.PageSize)
                .Take(filter.PageSize)
                .ToListAsync();

            return new PagedResult<ProductItemViewModel>
            {
                Items = items,
                TotalItems = totalItems,
                Page = filter.Page,
                PageSize = filter.PageSize
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

        public async Task<ProductVariantEdit> GetVariant(long id)
        {
            var product = context.ProductVariants
                .Include(p => p.ProductImages)
                .Include(p => p.ProductSize)
                .Include(p => p.Category)
                .Include(p => p.ProductIngredients)
                    .ThenInclude(v => v.Ingredient)
                .AsQueryable();

            if (context.ProductVariants.Where(v => v.Id == id).Count() < 1)
                throw new Exception("Product variant not found");

            var model = mapper.Map<ProductVariantEdit>(product.Where(p => p.Id == id).First());
            model.Slug = context.Products.Where(p => p.ProductVariants.Where(v => v.Id == model.Id).Any()).First().Slug;
            return model;
        }

        public async Task<long> CreateProduct(ProductCreateModel model)
        {
            var existing = await context.Products
                .Include(p => p.ProductVariants)
                .FirstOrDefaultAsync(p => p.Slug == model.Slug);

            var variant = new ProductVariantEntity
            {
                Name = model.Name,
                Price = model.Price,
                Weight = model.Weight,
                CategoryId = model.CategoryId,
                ProductSizeId = model.ProductSizeId
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

            for (int i = 0, im = 0; i < model.IngredientIds.Count; i++)
            {
                var id = model.IngredientIds[i];

                var ingredient = await context.Ingredients
                    .FirstOrDefaultAsync(i => i.Id == id);

                context.ProductIngredients.Add(new ProductIngredientEntity
                {
                    ProductVariantId = variant.Id,
                    IngredientId = ingredient.Id
                });
            }

            short priority = 1;
            foreach (var image in model.ImageFiles)
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

        public async Task<IEnumerable<IngredientModel>> GetIngredientsAsync()
        {
            var ingredients = await context.Ingredients
                .ProjectTo<IngredientModel>(mapper.ConfigurationProvider)
                .ToListAsync();
            return ingredients;
        }

        public async Task<IEnumerable<ProductSizeModel>> GetSizesAsync()
        {
            var sizes = await context.ProductSizes
                .ProjectTo<ProductSizeModel>(mapper.ConfigurationProvider)
                .ToListAsync();
            return sizes;
        }

        public async Task<long> EditProduct(ProductEditModel model)
        {
            var variant = await context.ProductVariants
                .Include(v => v.Product)
                .Include(v => v.ProductIngredients)
                .Include(v => v.ProductImages)
                .FirstOrDefaultAsync(v => v.Id == model.VariantID);

            if (variant == null)
                throw new Exception("Product variant not found");

            variant.Name = model.Name ?? variant.Name;
            variant.Price = model.Price ?? variant.Price;
            variant.Weight = model.Weight ?? variant.Weight;
            variant.CategoryId = model.CategoryId ?? variant.CategoryId;
            variant.ProductSizeId = model.ProductSizeId ?? variant.ProductSizeId;
            variant.ProductId =
                String.IsNullOrEmpty(model.Slug) ?
                    variant.ProductId :
                    (model.Slug == variant.Product.Slug ?
                        variant.ProductId :
                        context.Products.FirstOrDefault(p => p.Slug == model.Slug)?.Id ?? variant.ProductId);

            if (model.IngredientIds != null)
            {
                var existingIngredients = context.ProductIngredients
                    .Where(pi => pi.ProductVariantId == variant.Id);
                context.ProductIngredients.RemoveRange(existingIngredients);

                foreach (var ingredientId in model.IngredientIds ?? new List<long>())
                {
                    context.ProductIngredients.Add(new ProductIngredientEntity
                    {
                        ProductVariantId = variant.Id,
                        IngredientId = ingredientId
                    });
                }
            }

            var imgDelete = variant.ProductImages
            .Where(x => !model.ImageFiles!.Any(y => y.FileName == x.Name))
            .ToList();

            foreach (var img in imgDelete)
            {
                var productImage = await context.ProductImages
                    .Where(x => x.Id == img.Id)
                    .SingleOrDefaultAsync();
                if (productImage != null)
                {
                    await imageService.DeleteImageAsync(productImage.Name);
                    context.ProductImages.Remove(productImage);
                }
                context.SaveChanges();
            }

            short p = 0;
            //Перебираємо усі фото і їх зберігаємо або оновляємо
            foreach (var imgFile in model.ImageFiles!)
            {
                if (imgFile.ContentType == "old-image")
                {
                    var img = await context.ProductImages
                        .Where(x => x.Name == imgFile.FileName)
                        .SingleOrDefaultAsync();
                    img.Priority = p;
                    context.SaveChanges();
                }

                else
                {
                    try
                    {
                        var productImage = new ProductImageEntity
                        {
                            ProductVariantId = variant.Id,
                            Name = await imageService.SaveImageAsync(imgFile),
                            Priority = p
                        };
                        context.ProductImages.Add(productImage);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("Error Json Parse Data for PRODUCT IMAGE", ex.Message);
                    }
                }

                p++;

            }

            //if (model.ImageFiles != null && model.ImageFiles?.Count > 0)
            //{
            //    var oldImages = context.ProductImages
            //        .Where(img => img.ProductVariantId == variant.Id);
            //    foreach (var item in oldImages)
            //    {
            //        await imageService.DeleteImageAsync(item.Name);
            //    }
            //    context.ProductImages.RemoveRange(oldImages);

            //    short priority = 1;
            //    foreach (var image in model.ImageFiles)
            //    {
            //        var savedImage = await imageService.SaveImageAsync(image);

            //        context.ProductImages.Add(new ProductImageEntity
            //        {
            //            ProductVariantId = variant.Id,
            //            Name = savedImage,
            //            Priority = priority++
            //        });
            //    }
            //}

            await context.SaveChangesAsync();

            return variant.ProductId;
        }

        public async Task<bool> DeleteProductVariant(long id)
        {
            var product = await context.ProductVariants
                .Include(p => p.ProductImages)
                .Include(p => p.Product)
                    .ThenInclude(product => product.ProductVariants)
                .FirstOrDefaultAsync(p => p.Id == id);
            if (product == null)
                return false;
            foreach (var image in product.ProductImages)
            {
                await imageService.DeleteImageAsync(image.Name);
            }
            context.ProductVariants.Remove(product);
            if (product.Product.ProductVariants.Count() <= 1)
            {
                context.Products.Remove(product.Product);
            }
            await context.SaveChangesAsync();
            return true;

        }

        public async Task<IngredientModel> UploadIngredient(CreateIngredientModel model)
        {
            var entity = mapper.Map<IngredientEntity>(model);
            entity.Image = await imageService.SaveImageAsync(model.Image!);
            context.Ingredients.Add(entity);
            await context.SaveChangesAsync();

            return mapper.Map<IngredientModel>(entity);
        }


    }
}
