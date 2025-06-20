using AutoMapper;
using Core.Interface;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Domain;
using Domain.Entities;
using Core.Models.Category;
using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Restucode.Constants;

namespace Restucode.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriesController(ICategoryService categoryService) : ControllerBase
    {
        [HttpGet("pagedlist")]
        public async Task<IActionResult> PagedList([FromQuery] int page = 1, [FromQuery] int pageSize = 10, [FromQuery] string? search = null)
        {
            var model = await categoryService.ListAsyncPaged(page, pageSize, search);
            return Ok(model);
        }

        [HttpGet("list")]
        public async Task<IActionResult> List()
        {
            var model = await categoryService.ListAsync();
            return Ok(model);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromForm] CategoryAddModel model)
        {
            var category = await categoryService.Create(model);

            return Ok(category);
        }
        
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(long id)
        {

            var model = await categoryService.GetItemById((int)id);
            return Ok(model);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Edit(long id, [FromForm] CategoryEditModel model)
        {
            var category = await categoryService.Edit(id, model);
            return Ok(category);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(long id)
        {
            var category = await categoryService.Delete(id);
            return Ok(category);
        }

    }
}
