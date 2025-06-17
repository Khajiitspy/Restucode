using Core.Models.Product;

namespace Core.Models.Order;

public class OrderItemModel
{
    public long Id { get; set; }
    public string ProductVariantName { get; set; } = default!;
    public int Count { get; set; }
    public decimal PriceBuy { get; set; }
    public decimal Total => PriceBuy * Count;
}