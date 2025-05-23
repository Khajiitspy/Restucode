using AutoMapper;
using Restucode.Interface;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Restucode.Data;
using Restucode.Data.Entities;
using Restucode.Models.Category;
using FluentValidation;

namespace Restucode.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriesController(RestucodeDBContext RestucodeContext,
        IMapper mapper, IImageService imageService) : ControllerBase
    {
        [HttpGet]
        public async Task<IActionResult> List()
        {
            var model = await mapper.ProjectTo<CategoryItemViewModel>(RestucodeContext.Categories)
                .ToListAsync();
            return Ok(model);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromForm] CategoryAddModel model)
        {
            if (!ModelState.IsValid)
            {
                return ValidationProblem(ModelState);
            }

            var entity = mapper.Map<CategoryEntity>(model);
            entity.Image = await imageService.SaveImageAsync(model.Image);

            RestucodeContext.Categories.Add(entity);
            await RestucodeContext.SaveChangesAsync();

            return CreatedAtAction(nameof(List), new { id = entity.Id }, model);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(long id)
        {
            var category = await RestucodeContext.Categories.FindAsync(id);
            if (category == null)
                return NotFound();

            var model = mapper.Map<CategoryItemViewModel>(category);
            return Ok(model);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Edit(long id, [FromForm] CategoryEditModel model)
        {
            var category = await RestucodeContext.Categories.FindAsync(id);
            if (category == null)
                return NotFound();

            mapper.Map(model, category);

            if (model.Image != null)
            {
                category.Image = await imageService.SaveImageAsync(model.Image);
            }

            await RestucodeContext.SaveChangesAsync();
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(long id)
        {
            var category = await RestucodeContext.Categories.FindAsync(id);
            if (category == null)
                return NotFound();

            RestucodeContext.Categories.Remove(category);
            await RestucodeContext.SaveChangesAsync();

            return NoContent();
        }

    }
}
