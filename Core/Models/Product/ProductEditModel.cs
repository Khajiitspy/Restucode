using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Models.Product
{
    public class ProductEditModel
    {
        public long VariantID { get; set; }
        public string? Name { get; set; } = string.Empty;
        public string? Slug { get; set; } = string.Empty;
        public decimal? Price { get; set; }
        public int? Weight { get; set; }
        public long? CategoryId { get; set; }
        public long? ProductSizeId { get; set; }
        public List<long>? IngredientIds { get; set; }

        /// <summary>
        /// List of uploaded image files for the product.
        /// </summary>
        [BindProperty(Name = "imageFiles[]")]
        public List<IFormFile>? ImageFiles { get; set; }
    }


}
