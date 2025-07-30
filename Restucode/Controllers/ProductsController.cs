using Core.Interface;
using Core.Models.Product;
using Core.Models.General;
using Core.Services;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace Restucode.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController(IProductService productService) : ControllerBase
    {
        [HttpGet("list")]
        public async Task<ActionResult<PagedResult<ProductItemViewModel>>> List([FromQuery] ProductSearchModel filter)
        {
            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();
            var model = await productService.List(filter);
            stopWatch.Stop();
            // Get the elapsed time as a TimeSpan value.
            TimeSpan ts = stopWatch.Elapsed;

            // Format and display the TimeSpan value.
            string elapsedTime = String.Format("{0:00}:{1:00}:{2:00}.{3:00}",
                ts.Hours, ts.Minutes, ts.Seconds,
                ts.Milliseconds / 10);
            Console.WriteLine("-----------Elapsed Time------------: " + elapsedTime);

            return Ok(model);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Details(long id)
        {
            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();
            var model = await productService.Details(id);
            stopWatch.Stop();
            // Get the elapsed time as a TimeSpan value.
            TimeSpan ts = stopWatch.Elapsed;

            // Format and display the TimeSpan value.
            string elapsedTime = String.Format("{0:00}:{1:00}:{2:00}.{3:00}",
                ts.Hours, ts.Minutes, ts.Seconds,
                ts.Milliseconds / 10);
            Console.WriteLine("-----------Elapsed Time------------: " + elapsedTime);

            return Ok(model);
        }

        [HttpGet("variant/{id}")]
        public async Task<IActionResult> GetVariant(long id)
        {
            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();
            var model = await productService.GetVariant(id);
            stopWatch.Stop();
            // Get the elapsed time as a TimeSpan value.
            TimeSpan ts = stopWatch.Elapsed;

            // Format and display the TimeSpan value.
            string elapsedTime = String.Format("{0:00}:{1:00}:{2:00}.{3:00}",
                ts.Hours, ts.Minutes, ts.Seconds,
                ts.Milliseconds / 10);
            Console.WriteLine("-----------Elapsed Time------------: " + elapsedTime);
            return Ok(model);
        }

        [HttpPost("create")]
        public async Task<IActionResult> CreateProduct([FromForm] ProductCreateModel model)
        {
            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();
            var salo = Request.Form;
            if (model.ImageFiles == null)
                return BadRequest("Image files are empty!");
            if (model.IngredientIds == null)
                return BadRequest("Product ingredients are empty!");
            var entity = await productService.CreateProduct(model);
            stopWatch.Stop();
            // Get the elapsed time as a TimeSpan value.
            TimeSpan ts = stopWatch.Elapsed;

            // Format and display the TimeSpan value.
            string elapsedTime = String.Format("{0:00}:{1:00}:{2:00}.{3:00}",
                ts.Hours, ts.Minutes, ts.Seconds,
                ts.Milliseconds / 10);
            Console.WriteLine("-----------Elapsed Time------------: " + elapsedTime);

            if (entity != null)
                return Ok(model);
            else return BadRequest("Error create product!");
        }

        [HttpGet("sizes")]
        public async Task<IActionResult> GetSizes()
        {
            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();
            var sizes = await productService.GetSizesAsync();
            stopWatch.Stop();
            // Get the elapsed time as a TimeSpan value.
            TimeSpan ts = stopWatch.Elapsed;

            // Format and display the TimeSpan value.
            string elapsedTime = String.Format("{0:00}:{1:00}:{2:00}.{3:00}",
                ts.Hours, ts.Minutes, ts.Seconds,
                ts.Milliseconds / 10);
            Console.WriteLine("-----------Elapsed Time------------: " + elapsedTime);

            return Ok(sizes);
        }

        [HttpGet("ingredients")]
        public async Task<IActionResult> GetIngredients()
        {
            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();
            var ingredients = await productService.GetIngredientsAsync();
            stopWatch.Stop();
            // Get the elapsed time as a TimeSpan value.
            TimeSpan ts = stopWatch.Elapsed;

            // Format and display the TimeSpan value.
            string elapsedTime = String.Format("{0:00}:{1:00}:{2:00}.{3:00}",
                ts.Hours, ts.Minutes, ts.Seconds,
                ts.Milliseconds / 10);
            Console.WriteLine("-----------Elapsed Time------------: " + elapsedTime);

            return Ok(ingredients);
        }

        [HttpPut("edit")]
        public async Task<IActionResult> Edit([FromForm] ProductEditModel model)
        {
            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();
            var updatedId = await productService.EditProduct(model);
            stopWatch.Stop();
            // Get the elapsed time as a TimeSpan value.
            TimeSpan ts = stopWatch.Elapsed;

            // Format and display the TimeSpan value.
            string elapsedTime = String.Format("{0:00}:{1:00}:{2:00}.{3:00}",
                ts.Hours, ts.Minutes, ts.Seconds,
                ts.Milliseconds / 10);
            Console.WriteLine("-----------Elapsed Time------------: " + elapsedTime);

            return Ok(updatedId);
        }

        [HttpDelete("remove/{id}")]
        public async Task<IActionResult> Delete(long id)
        {
            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();
            var result = await productService.DeleteProductVariant(id);
            stopWatch.Stop();
            // Get the elapsed time as a TimeSpan value.
            TimeSpan ts = stopWatch.Elapsed;

            // Format and display the TimeSpan value.
            string elapsedTime = String.Format("{0:00}:{1:00}:{2:00}.{3:00}",
                ts.Hours, ts.Minutes, ts.Seconds,
                ts.Milliseconds / 10);
            Console.WriteLine("-----------Elapsed Time------------: " + elapsedTime);

            if (result)
                return Ok("Product deleted successfully.");
            else
                return BadRequest("Error deleting product.");
        }

        [HttpPost("ingredients")]
        public async Task<IActionResult> CreateIngredient([FromForm] CreateIngredientModel model)
        {
            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();
            var ingredient = await productService.UploadIngredient(model);
            stopWatch.Stop();
            // Get the elapsed time as a TimeSpan value.
            TimeSpan ts = stopWatch.Elapsed;

            // Format and display the TimeSpan value.
            string elapsedTime = String.Format("{0:00}:{1:00}:{2:00}.{3:00}",
                ts.Hours, ts.Minutes, ts.Seconds,
                ts.Milliseconds / 10);
            Console.WriteLine("-----------Elapsed Time------------: " + elapsedTime);

            if (ingredient != null)
                return Ok(ingredient);
            return BadRequest();
        }
    }
}
