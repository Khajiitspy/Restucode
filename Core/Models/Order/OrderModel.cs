namespace Core.Models.Order;

public class OrderModel
{
    public long Id { get; set; }
    public string Status { get; set; } = default!;
    public DateTime CreatedAt { get; set; }
    public List<OrderItemModel> Items { get; set; } = new();
    public decimal Total => Items.Sum(i => i.Total);
}