using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Domain.Entities.Identity;

namespace Domain.Entities;

public class CartEntity
{
    [ForeignKey("ProductVariant")]
    public long ProductVariantId { get; set; }
    [ForeignKey("User")]
    public long UserId { get; set; }

    [Range(0, 50)]
    public int Quantity { get; set; }

    public virtual ProductVariantEntity? ProductVariant { get; set; }
    public virtual UserEntity? User { get; set; }
}