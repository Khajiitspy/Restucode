namespace Core.Models.Cart;

public class CartCreateUpdateModel
{
    public long ProductVariantId { get; set; }
    public int Quantity { get; set; } = 1;
}