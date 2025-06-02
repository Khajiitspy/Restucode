using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    [Table("tblProductSizes")]
    public class ProductSizeEntity: BaseEntity<long>
    {
        [StringLength(250)]
        public string Name { get; set; } = string.Empty;// 50 cm or 40 cm

        public ICollection<ProductEntity>? Products { get; set; }
    }
}
