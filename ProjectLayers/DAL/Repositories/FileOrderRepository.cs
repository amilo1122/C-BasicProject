using DAL.Generators;
using Shared.Interfaces;
using Shared.Models;

namespace DAL.Repositories
{
    public class FileOrderRepository : IOrderRepository
    {
        private List<Order> _orders = new List<Order>();
        private List<OrderItems> _orderItems = new List<OrderItems>();
        BaseRepository orderIndex = new BaseRepository();
        BaseRepository orderItemsIndex = new BaseRepository();

        public int Add(int userId, decimal totalSum)
        {
            Order order = new Order();
            order.Id = orderIndex.IncrementOrderIndex();
            order.UserId = userId;
            order.TotalSum = totalSum;
            order.CreatedDate = DateTime.Now;
            _orders.Add(order);
            return order.Id;
        }

        public int Add(int userId, decimal totalSum, int id, DateTime date)
        {
            Order order = new Order();
            order.Id = id;
            order.UserId = userId;
            order.TotalSum = totalSum;
            order.CreatedDate = date;
            _orders.Add(order);
            return order.Id;
        }

        public void Delete(int orderId)
        {
            if(_orders.Where(x => x.Id == orderId).ToList().Count > 0)
            {
                var order = _orders.Single(x => x.Id == orderId);
                _orders.Remove(order);
            }
        }

        public void AddOrderItems(int orderId, int goodId, decimal price, int quantity)
        {
            OrderItems orderItems = new OrderItems();
            orderItems.Id = orderItemsIndex.IncrementOrderItemsIndex();
            orderItems.OrderId = orderId;
            orderItems.GoodId = goodId;
            orderItems.Price = price;
            orderItems.Quantity = quantity;
            _orderItems.Add(orderItems);
        }

        public void DeleteOrderItems(int orderId)
        {
            var orderItems = _orderItems.Where(x => x.Id == orderId).ToList();
            if (orderItems.Count > 0)
            {
                foreach (var item in orderItems)
                {
                    _orderItems.Remove(item);
                }
            }
        }

        public List<Order> GetUserOrders(int userdId)
        {
            return _orders.Where(x => x.UserId == userdId).ToList();
        }

        public List<Order> GetAllOrders()
        {
            return _orders;
        }

        public List<OrderItems> GetOrderItems(int orderId)
        {
            return _orderItems.Where(x => x.OrderId == orderId).ToList();
        }

        public List<OrderItems> GetAllOrdersItems()
        {
            return _orderItems;
        }
                
    }
}
