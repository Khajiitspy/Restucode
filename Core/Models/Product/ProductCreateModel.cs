using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Models.Product
{
    public class ProductCreateModel
    {
        public string Slug { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public int Weight { get; set; }
        public string Category { get; set; } = string.Empty;
        public string? Size { get; set; }

        public List<string> IngredientNames { get; set; } = new();
        public List<IFormFile> IngredientImages { get; set; } = new();
        public List<IFormFile> Images { get; set; } = new();
    }


}
