using Dapper;
using Npgsql;
using Shared.Interfaces;
using Shared.Models;

namespace DAL.Repositories
{
    public class DapperOrdersRepository : IOrdersRepository
    {
        // Создаем строку подключения к БД
        string connectionString = Config.SQLConnectionString;

        // Добавляем новый заказ, возвращаем id созданного заказа
        public int AddOrder(long userId, decimal totalSum)
        {
            using (var connection = new NpgsqlConnection(connectionString))
            {
                var createdDate = DateTime.Now;
                var sql = @"INSERT INTO Orders (userid, totalsum, createddate) 
                            VALUES (" + userId + "," + totalSum + ",'" + createdDate + "');";
                var result = connection.QueryFirstOrDefault<Order>(sql);
                sql = @"SELECT id 
                        FROM orders 
                        WHERE userid = " + userId + " AND totalsum = " + totalSum + " AND createddate = '" + createdDate + "'";
                return connection.QueryFirstOrDefault<Order>(sql).Id;
            }
        }

        // Создаем элементы заказа
        public void AddOrderItems(int orderId, int goodId, decimal price, int quantity)
        {
            using (var connection = new NpgsqlConnection(connectionString))
            {
                var sql = @"INSERT INTO OrderItems (orderid, goodid, price, quantity) 
                            VALUES (" + orderId + "," + goodId + "," + price + "," + quantity + ");";
                var result = connection.QueryFirstOrDefault<OrderItems>(sql);
            }
        }

        // Возвращаем заказ по его id
        public Order GetOrder(int id)
        {
            using (var connection = new NpgsqlConnection(connectionString))
            {
                var sql = @"SELECT *
                            FROM orders
                            WHERE id = " + id;
                return connection.QueryFirstOrDefault<Order>(sql);
            }
        }

        // Возвращаем список элементов заказа по id заказа
        public List<OrderItems> GetOrderItems(int orderId)
        {
            using (var connection = new NpgsqlConnection(connectionString))
            {
                var sql = @"SELECT id, orderid, goodid, price , quantity
                            FROM orderitems
                            WHERE orderid = " + orderId;
                var result = connection.Query<OrderItems>(sql);
                return result.ToList();
            }
        }

        // Возвращаем список заказов пользователя
        public List<Order> GetUserOrders(long userId)
        {
            using (var connection = new NpgsqlConnection(connectionString))
            {
                var sql = @"SELECT * FROM orders WHERE userid = " + userId;
                var result = connection.Query<Order>(sql);
                return result.ToList();
            };
        }
    }
}
