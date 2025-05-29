using Microsoft.AspNetCore.Http;

namespace Core.Models.Category
{
    public class CategoryAddModel
    {
        public string Name { get; set; } = String.Empty;
        public IFormFile Image { get; set; } = null;
    }
}
