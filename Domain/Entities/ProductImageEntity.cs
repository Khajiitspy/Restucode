using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    [Table("tblProductImages")]
    public class ProductImageEntity : BaseEntity<long>
    {
        [StringLength(250)]
        public string Name { get; set; } = string.Empty;

        public short Priority { get; set; }

        [ForeignKey("ProductVariant")]
        public long ProductVariantId { get; set; }
        public ProductVariantEntity? ProductVariant { get; set; }
    }
}
