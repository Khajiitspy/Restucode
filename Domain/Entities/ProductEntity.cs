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
        public ICollection<ProductVariantEntity>? ProductVariants { get; set; } = new List<ProductVariantEntity>();

        [StringLength(250)]
        public string Slug { get; set; } = string.Empty;
    }
}
