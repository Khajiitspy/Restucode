using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities
{
    [Table("tblProductIngredients")]
    public class ProductIngredientEntity
    {
        [ForeignKey("ProductVariant")]
        public long ProductVariantId { get; set; }

        [ForeignKey("Ingredient")]
        public long IngredientId { get; set; }

        public virtual ProductVariantEntity? ProductVariant { get; set; }
        public virtual IngredientEntity? Ingredient { get; set; }
    }

}
