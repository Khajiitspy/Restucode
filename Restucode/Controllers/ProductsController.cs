using Core.Interface;
using Core.Models.Product;
using Core.Services;
using Microsoft.AspNetCore.Mvc;

namespace Restucode.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController(IProductService productService): ControllerBase
    {
        [HttpGet]
        public async Task<IActionResult> List([FromQuery] string? search, int page = 1, int pageSize = 5)
        {
            var model = await productService.List(search, page, pageSize);
            return Ok(model);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Details(long id)
        {
            var model = await productService.Details(id);
            return Ok(model);
        }

        [HttpPost]
        public async Task<IActionResult> CreateProduct([FromForm]ProductCreateModel model)
        {
            long id = await productService.CreateProduct(model);

            return Ok(id);
        }

    }
}
