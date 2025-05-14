using AutoMapper;
using Restucode.Interface;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Restucode.Data;
using Restucode.Data.Entities;
using Restucode.Models.Category;

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
                return BadRequest(ModelState);
            }
            var entity = mapper.Map<CategoryEntity>(model);
            entity.Image = await imageService.SaveImageAsync(model.Image);

            RestucodeContext.Categories.Add(entity);
            await RestucodeContext.SaveChangesAsync();
            return CreatedAtAction(nameof(List), new { id = entity.Id }, model);
        }
    }
}
