
using Shared.Models;

namespace Shared.Interfaces
{
    public interface ICartsRepository
    {
        bool Add(long userId, int goodId, int quantity);
        bool Delete(long userId, int goodId);
        bool UpdateQuantity(long userId, int goodId, int quantity);
        List<Cart> GetUserCart(long userId);
    }
}
