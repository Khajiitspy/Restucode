using Core.Models.Cart;

namespace Core.Interface;

public interface ICartService
{
    Task CreateUpdate(CartCreateUpdateModel updateModel);
    Task RemoveFromCart(long ProductVariantId);
    public Task<List<CartItemModel>> GetCartItems();
}