using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    [Table("tblProductVariants")]
    public class ProductVariantEntity: BaseEntity<long>
    {
        [StringLength(250)]
        public string Name { get; set; } = string.Empty;

        public decimal Price { get; set; }

        public int Weight { get; set; }

        [ForeignKey("Category")]
        public long CategoryId { get; set; }

        public CategoryEntity? Category { get; set; } = null!;

        [ForeignKey("ProductSize")]
        public long? ProductSizeId { get; set; }

        public ProductSizeEntity? ProductSize { get; set; } = null!;

        [ForeignKey("Product")]
        public long ProductId { get; set; }

        public ProductEntity? Product { get; set; } = null!;

        public ICollection<ProductIngredientEntity>? ProductIngredients { get; set; }

        public ICollection<ProductImageEntity> ProductImages { get; set; } = new List<ProductImageEntity>();
        public ICollection<CartEntity>? Carts { get; set; }
    }
}
