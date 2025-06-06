using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Models.Product
{
    public class ProductVariant
    {
        public long Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public int Weight { get; set; }
        public string Category { get; set; } = string.Empty; // If the category is something like "Vegan", a variant could have a different category.
        public string? Size { get; set; }

        public List<IngredientModel> Ingredients { get; set; } = new(); // For variants like "With Cheese", "Without Onions", "Vegan" etc.
        public List<string> Images { get; set; } = new(); // Different images for each variant
    }
}
