using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Models.Product
{
    public class ProductDetailsViewModel
    {
        public long Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public int Weight { get; set; }
        public string Slug { get; set; } = string.Empty;

        public string Category { get; set; } = string.Empty;
        public string? Size { get; set; }

        public List<IngredientModel> Ingredients { get; set; } = new();
        public List<string> Images { get; set; } = new();
    }
}
