using Core.Models.Cart;

namespace Core.Interface;

public interface ICartService
{
    Task CreateUpdate(CartCreateUpdateModel updateModel);
    public Task<List<CartItemModel>> GetCartItems();
}