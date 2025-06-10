using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Models.Product
{
    public class ProductVariantEdit
    {
        public long Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Slug { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public int Weight { get; set; }
        public long CategoryId { get; set; } // If the category is something like "Vegan", a variant could have a different category.
        public long? ProductSizeId { get; set; }

        public List<long> IngredientIds { get; set; } = new(); // For variants like "With Cheese", "Without Onions", "Vegan" etc.
        public List<string> ProductImages { get; set; } = new(); // Different images for each variant
    }
}
