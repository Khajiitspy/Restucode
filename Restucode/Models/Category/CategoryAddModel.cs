namespace Restucode.Models.Category
{
    public class CategoryAddModel
    {
        public string Name { get; set; } = String.Empty;
        public IFormFile Image { get; set; } = null;
    }
}
