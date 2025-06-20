using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace Core.Models.Category
{
    public class CategoryAddModel
    {
        [Required]
        public string Name { get; set; } = String.Empty;
        [Required]
        public IFormFile Image { get; set; } = default!;

    }
}
