using Core.Models.Cart;
using Core.Models.Order;

namespace Core.Interface;

public interface ICartService
{
    Task CreateUpdate(CartCreateUpdateModel updateModel);
    Task RemoveFromCart(long ProductVariantId);
    public Task<List<CartItemModel>> GetCartItems();
    public Task OrderCart(OrderInformation info);
}
