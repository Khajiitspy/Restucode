using AutoMapper;
using Core.Interface;
using Core.Models.Category;
using Core.Models.General;
using Domain;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Services
{
    public class CategoryService(RestucodeDBContext context, IMapper mapper, IImageService imageService) : ICategoryService
    {
        public async Task<CategoryItemViewModel> Create(CategoryAddModel model)
        {
            var entity = mapper.Map<CategoryEntity>(model);
            entity.Image = await imageService.SaveImageAsync(model.Image!);
            await context.Categories.AddAsync(entity);
            await context.SaveChangesAsync();
            var item = mapper.Map<CategoryItemViewModel>(entity);
            return item;
        }

        public async Task<bool> Delete(long Id)
        {
            var category = await context.Categories.FindAsync(Id);
            if (category == null)
                return false;

            context.Categories.Remove(category);
            await context.SaveChangesAsync();

            return true;
        }

        public async Task<CategoryItemViewModel?> GetItemById(long id)
        {
            var model = await mapper
                .ProjectTo<CategoryItemViewModel>(context.Categories.Where(x => x.Id == id))
                .SingleOrDefaultAsync();
            return model;
        }

        public async Task<PagedResult<CategoryItemViewModel>> ListAsync(int page, int pageSize, string? search)
        {
            var query = context.Categories.AsQueryable();

            if (!string.IsNullOrEmpty(search))
            {
                query = query.Where(c => c.Name.Contains(search));
            }

            var totalItems = await query.CountAsync();

            var items = await mapper.ProjectTo<CategoryItemViewModel>(query)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return new PagedResult<CategoryItemViewModel>
            {
                Items = items,
                TotalItems = totalItems,
                Page = page,
                PageSize = pageSize
            };
        }


        public async Task<CategoryItemViewModel> Edit(CategoryEditModel model)
        {
            var existing = await context.Categories.FirstOrDefaultAsync(x => x.Id == model.Id);

            existing = mapper.Map(model, existing);

            if (model.Image != null)
            {
                await imageService.DeleteImageAsync(existing.Image);
                existing.Image = await imageService.SaveImageAsync(model.Image);
            }
            await context.SaveChangesAsync();

            var item = mapper.Map<CategoryItemViewModel>(existing);
            return item;
        }
    }
}
