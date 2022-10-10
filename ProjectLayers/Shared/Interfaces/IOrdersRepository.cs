using Shared.Models;

namespace Shared.Interfaces
{
    public interface IOrdersRepository
    {
        int AddOrder(long userId, decimal totalSum);
        void AddOrderItems(int orderId, int goodId, decimal price, int quantity);
        List<Order> GetUserOrders(long userId);
        List<OrderItems> GetOrderItems(int orderId);
        Order GetOrder(int id);
    }
}
