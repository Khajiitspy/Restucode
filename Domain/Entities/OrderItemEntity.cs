using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Domain.Entities.Identity;

namespace Domain.Entities;

[Table("tblOrderItems")]
public class OrderItemEntity : BaseEntity<long>
{
    public decimal PriceBuy { get; set; }
    public int Count { get; set; }
    [ForeignKey("ProductVariant")]
    public long ProductVariantId { get; set; }
    [ForeignKey(nameof(Order))]
    public long OrderId { get; set; }
    public virtual ProductVariantEntity? ProductVariant { get; set; }
    public virtual OrderEntity? Order { get; set; }
}