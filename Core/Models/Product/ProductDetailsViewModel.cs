using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Models.Product
{
    public class ProductDetailsViewModel
    {
        public List<ProductVariant> Variants { get; set; } = new();
        public long Id { get; set; }
        public string Slug { get; set; } = string.Empty;
    }
}
