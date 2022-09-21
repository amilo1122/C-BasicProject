
using Shared.Models;

namespace Shared.Interfaces
{
    public interface ICartRepository
    {
        void Add(long userId, int goodsId, int quantity);
        void Add(long userId, int goodsId, int quantity, int id);
        void Remove(long userId, int goodsId);
        void UpdateQuantity(long userId, int goodsId, int quantity);
        List<Cart> GetUserCart(long userId);
        List<Cart> GetAllCart();
    }
}
