using Core.Interface;
using Core.Models.Product;
using Core.Services;
using Microsoft.AspNetCore.Mvc;

namespace Restucode.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController(IProductService productService) : ControllerBase
    {
        [HttpGet("list")]
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

        [HttpGet("variant/{id}")]
        public async Task<IActionResult> GetVariant(long id)
        {
            var model = await productService.GetVariant(id);
            return Ok(model);
        }

        [HttpPost("create")]
        public async Task<IActionResult> CreateProduct([FromForm] ProductCreateModel model)
        {
            var salo = Request.Form;
            if (model.ImageFiles == null)
                return BadRequest("Image files are empty!");
            if (model.IngredientIds == null)
                return BadRequest("Product ingredients are empty!");
            var entity = await productService.CreateProduct(model);
            if (entity != null)
                return Ok(model);
            else return BadRequest("Error create product!");
        }

        [HttpGet("sizes")]
        public async Task<IActionResult> GetSizes()
        {
            var sizes = await productService.GetSizesAsync();

            return Ok(sizes);
        }

        [HttpGet("ingredients")]
        public async Task<IActionResult> GetIngredients()
        {
            var ingredients = await productService.GetIngredientsAsync();

            return Ok(ingredients);
        }

        [HttpPut("edit")]
        public async Task<IActionResult> Edit([FromForm] ProductEditModel model)
        {
            var updatedId = await productService.EditProduct(model);
            return Ok(updatedId);
        }

        [HttpDelete("remove/{id}")]
        public async Task<IActionResult> Delete(long id)
        {
            var result = await productService.DeleteProductVariant(id);
            if (result)
                return Ok("Product deleted successfully.");
            else
                return BadRequest("Error deleting product.");
        }

        [HttpPost("ingredients")]
        public async Task<IActionResult> CreateIngredient([FromForm] CreateIngredientModel model)
        {
            var ingredient = await productService.UploadIngredient(model);
            if (ingredient != null)
                return Ok(ingredient);
            return BadRequest();
        }
    }
}
