
using Shared.Models;

namespace Shared.Interfaces
{
    public interface ICartRepository
    {
        void Add(int userId, int goodsId, int quantity);
        void Add(int userId, int goodsId, int quantity, int id);
        void Remove(int userId, int goodsId);
        void UpdateQuantity(int userId, int goodsId, int quantity);
        List<Cart> GetUserCart(int userId);
        List<Cart> GetAllCart();
    }
}
