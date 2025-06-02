using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    [Table("tblProducts")]
    public class ProductEntity : BaseEntity<long>
    {
        [StringLength(250)]
        public string Name { get; set; } = string.Empty;

        public decimal Price { get; set; }

        public int Weight { get; set; }

        [StringLength(250)]
        public string Slug { get; set; } = string.Empty;

        [ForeignKey("Category")]
        public long CategoryId { get; set; }

        public CategoryEntity? Category { get; set; } = null!;

        [ForeignKey("ProductSize")]
        public long? ProductSizeId { get; set; }

        public ProductSizeEntity? ProductSize { get; set; } = null!;

        public ICollection<ProductIngredientEntity>? ProductIngredients { get; set; }
        public ICollection<ProductImageEntity>? ProductImages { get; set; }
    }
}
