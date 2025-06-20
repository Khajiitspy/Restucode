using Microsoft.AspNetCore.Http;

namespace Core.Models.Category
{
    public class CategoryEditModel
    {
        public string Name { get; set; }
        public IFormFile? Image { get; set; }
    }
}
