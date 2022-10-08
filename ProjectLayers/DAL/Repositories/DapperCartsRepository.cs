using Dapper;
using Npgsql;
using Shared.Interfaces;
using Shared.Models;

namespace DAL.Repositories
{
    public class DapperCartsRepository : ICartRepository
    {
        // Создаем строку подключения к БД
        string connectionString = Config.SQLConnectionString;

        // Добавляем товар в корзину пользователя
        public void Add(long userId, int goodId, int quantity)
        {
            using (var connection = new NpgsqlConnection(connectionString))
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
            }
        }

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

        public List<Cart> GetAllCart()
        {
            throw new NotImplementedException();
        }

        public List<Cart> GetUserCart(long userId)
        {
            throw new NotImplementedException();
        }

        public bool UpdateQuantity(long userId, int goodId, int quantity)
        {
            throw new NotImplementedException();
        }
    }
}
