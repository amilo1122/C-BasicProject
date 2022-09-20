using Shared.Models;

namespace Shared.Interfaces
{
    public interface IOrderRepository
    {
        int Add(int userId, decimal totalSum);
        int Add(int userId, decimal totalSum, int id, DateTime date);
        void Delete(int orderId);
        void AddOrderItems(int orderId, int goodId, decimal price, int quantity);
        void DeleteOrderItems(int orderId);
        List<Order> GetUserOrders(int userdId);
        List<Order> GetAllOrders();
        List<OrderItems> GetOrderItems(int orderId);
        List<OrderItems> GetAllOrdersItems();
    }
}
