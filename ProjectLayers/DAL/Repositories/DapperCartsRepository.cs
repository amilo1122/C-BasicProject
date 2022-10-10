using Dapper;
using Npgsql;
using Shared.Interfaces;
using Shared.Models;

namespace DAL.Repositories
{
    public class DapperCartsRepository : ICartsRepository
    {
        // Создаем строку подключения к БД
        string connectionString = Config.SQLConnectionString;

        // Добавляем товар в корзину пользователя, если количество >= 0, false, если < 0. Если товар существует, увеличиваем количество товара
        public bool Add(long userId, int goodId, int quantity)
        {
            using (var connection = new NpgsqlConnection(connectionString))
            {
                if (quantity >= 0)
                {
                    var sql = @"SELECT id, quantity FROM carts WHERE userid = " + userId + " AND goodid = " + goodId;
                    var result = connection.QueryFirstOrDefault<Cart>(sql);

                    if (result == null)
                    {
                        sql = @"INSERT INTO carts (userid, goodid, quantity) 
                    VALUES (" + userId + "," + goodId + "," + quantity + ");";
                        result = connection.QueryFirstOrDefault<Cart>(sql);
                    }
                    else
                    {
                        var newQuantity = result.Quantity + quantity;
                        sql = @"UPDATE carts SET quantity = " + newQuantity + " WHERE id = " + result.Id;
                        result = connection.QueryFirstOrDefault<Cart>(sql);
                    }
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }

        // Удаляем товар из корзины пользователя
        public bool Delete(long userId, int goodId)
        {
            using (var connection = new NpgsqlConnection(connectionString))
            {
                try
                {
                    var sql = @"DELETE FROM carts WHERE userid = " + userId + " AND goodid = " + goodId;
                    var result = connection.QueryFirstOrDefault<User>(sql);
                    return true;
                }
                catch
                {
                    return false;
                }
            }
        }

        // Возвращаем корзину пользователя
        public List<Cart> GetUserCart(long userId)
        {
            using (var connection = new NpgsqlConnection(connectionString))
            {
                var sql = @"SELECT id, userid, goodid, quantity 
                            FROM carts
                            WHERE userid = " + userId;
                var result = connection.Query<Cart>(sql);
                return result.ToList();
            }
        }

        // Обновляем количество товара в корзине. Если сумма текущего и нового количества > 0, обновляем количество, = 0 - удаляем товар из корзины, < 0 - возвращем false
        public bool UpdateQuantity(long userId, int goodId, int quantity)
        {
            using (var connection = new NpgsqlConnection(connectionString))
            {
                var sql = @"SELECT id, quantity 
                            FROM carts
                            WHERE userid = " + userId + " AND goodid = " + goodId;
                var result = connection.QueryFirstOrDefault<Cart>(sql);
                if (result != null)
                {
                    var newQuantity = result.Quantity + quantity;
                    if (newQuantity > 0)
                    {
                        sql = @"UPDATE carts SET quantity = " + newQuantity + " WHERE id = " + result.Id;
                        result = connection.QueryFirstOrDefault<Cart>(sql);
                        return true;
                    }
                    else if (newQuantity == 0)
                    {
                        sql = @"DELETE FROM carts WHERE id = " + result.Id;
                        result = connection.QueryFirstOrDefault<Cart>(sql);
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
                else
                {
                    return false;
                }
            }
        }
    }
}
