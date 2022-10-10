using Dapper;
using Npgsql;
using Shared.Interfaces;
using Shared.Models;

namespace DAL.Repositories
{
    public class DapperGoodsRepository : IGoodsRepository
    {
        // Создаем строку подключения к БД
        string connectionString = Config.SQLConnectionString;

        // Добавляем новый товар, false в случае неуникальности наименования товара
        public bool Add(int categoryId, string name, string description, decimal price, int quantity, string url)
        {
            using (var connection = new NpgsqlConnection(connectionString))
            {
                try
                {
                    var sql = @"INSERT INTO Goods (name, description, categoryid, price, quantity, url) 
                                VALUES ('" + name + "', '" + description + "'," + categoryId + "," + price + "," + quantity + ", '" + url + "');";
                    var result = connection.QueryFirstOrDefault<Category>(sql);
                    return true;
                }
                catch
                {
                    return false;
                }
            }
        }

        // Меняем id категории товара по id, false, если id не найден 
        public bool ChangeCategoryId(int id, int categoryId)
        {
            using (var connection = new NpgsqlConnection(connectionString))
            {
                try
                {
                    var sql = @"UPDATE goods
                                SET categoryId = '" + categoryId + "' WHERE id = " + id;
                    var result = connection.QueryFirstOrDefault<Category>(sql);
                    return true;
                }
                catch
                {
                    return false;
                }
            }
                
        }

        // Меняем описание товара по id, false, если id не найден
        public bool ChangeDescription(int id, string description)
        {
            using (var connection = new NpgsqlConnection(connectionString))
            {
                try
                {
                    var sql = @"UPDATE goods
                                SET description = '" + description + "' WHERE id = " + id;
                    var result = connection.QueryFirstOrDefault<Category>(sql);
                    return true;
                }
                catch
                {
                    return false;
                }
            }
                
        }

        // Меняем url товара по id, false, если id не найден
        public bool ChangeGoodUrl(int id, string url)
        {
            using (var connection = new NpgsqlConnection(connectionString))
            {
                try
                {
                    var sql = @"UPDATE goods
                                SET url = '" + url + "' WHERE id = " + id;
                    var result = connection.QueryFirstOrDefault<Category>(sql);
                    return true;
                }
                catch
                {
                    return false;
                }
            }
                
        }

        //Меняем наименование товара по id, false, если id не найден
        public bool ChangeName(int id, string name)
        {
            using (var connection = new NpgsqlConnection(connectionString))
            {
                try
                {
                    var sql = @"UPDATE goods
                                SET name = '" + name + "' WHERE id = " + id;
                    var result = connection.QueryFirstOrDefault<Category>(sql);
                    return true;
                }
                catch
                {
                    return false;
                }
            }
                
        }

        //Меняем стоимость товара по id, false, если id не найден
        public bool ChangePrice(int id, decimal price)
        {
            using (var connection = new NpgsqlConnection(connectionString))
            {
                try
                {
                    var sql = @"UPDATE goods
                                SET price = " + price + " WHERE id = " + id;
                    var result = connection.QueryFirstOrDefault<Category>(sql);
                    return true;
                }
                catch
                {
                    return false;
                }
            }
                
        }

        // Меняем доступное количество товара по id, false, если id не найден
        public bool ChangeQuantity(int id, int quantity)
        {
            using (var connection = new NpgsqlConnection(connectionString))
            {
                if (quantity >= 0)
                {
                    try
                    {
                        var sql = @"UPDATE goods
                                    SET quantity = " + quantity + " WHERE id = " + id;
                        var result = connection.QueryFirstOrDefault<Category>(sql);
                        return true;
                    }
                    catch
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

        // Удаляем товар по id, false, если id не найден
        public bool Delete(string name)
        {
            using (var connection = new NpgsqlConnection(connectionString))
            {
                try
                {
                    var sql = @"DELETE FROM goods WHERE name LIKE '" + name + "'";
                    var result = connection.QueryFirstOrDefault<Category>(sql);
                    return true;
                }
                catch
                {
                    return false;
                }
            }                
        }

        // Выводим список всех товаров
        public List<Good> GetAllGoods()
        {
            using (var connection = new NpgsqlConnection(connectionString))
            {
                string sql = @"SELECT id, name, description, categoryId, price, quantity, url 
                                FROM goods";
                var result = connection.Query<Good>(sql);
                return result.ToList();
            }
        }

        // Выводим список товаров категории по ее id
        public List<Good> GetAllGoods(int categoryId)
        {
            using (var connection = new NpgsqlConnection(connectionString))
            {
                string sql = @"SELECT id, name, description, categoryId, price, quantity, url 
                                FROM goods
                                WHERE categoryid = " + categoryId;
                var result = connection.Query<Good>(sql);
                return result.ToList();
            }
        }

        // Выводим товар по id
        public Good GetGood(int id)
        {
            using (var connection = new NpgsqlConnection(connectionString))
            {
                string sql = @"SELECT id, name, description, categoryId, price, quantity, url 
                                FROM goods
                                WHERE id = " + id;
                return connection.QueryFirstOrDefault<Good>(sql);
            }
        }
    }
}
