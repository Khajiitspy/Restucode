using AutoMapper;
using Core.Interface;
using Core.Models.Category;
using Core.Models.General;
using Core.Models.Product;
using Domain;
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
                .Include(p => p.ProductImages)
                .Include(p => p.ProductSize)
                .Include(p => p.Category)
                .AsQueryable();

            if (!string.IsNullOrEmpty(search))
                query = query.Where(p => p.Name.Contains(search));

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
            var product = await context.Products
                .Include(p => p.Category)
                .Include(p => p.ProductSize)
                .Include(p => p.ProductImages)
                .Include(p => p.ProductIngredients)
                .ThenInclude(pi => pi.Ingredient)
                .FirstOrDefaultAsync(p => p.Id == id);

            var model = mapper.Map<ProductDetailsViewModel>(product);
            return model;
        }

    }
}
