using Dapper;
using Npgsql;
using Shared.Interfaces;
using Shared.Models;

namespace DAL.Repositories
{
    public class DapperCategoriesRepository : ICategoriesRepository
    {
        // Создаем строку подключения к БД
        string connectionString = Config.SQLConnectionString;

        // Добавляем новую категорию, false в случае неуникальности наименования категории
        public bool Add(string name)
        {
            using (var connection = new NpgsqlConnection(connectionString))
            {
                try
                {
                    var sql = @"INSERT INTO Categories (Name) 
                                VALUES ('" + name + "');";
                    var result = connection.QueryFirstOrDefault<Category>(sql);
                    return true;
                }
                catch
                {
                    return false;
                }
            }
        }

        // Выводим список всех категорий
        public List<Category> Browse()
        {
            using (var connection = new NpgsqlConnection(connectionString))
            {
                string sql = @"SELECT id, name 
                                FROM categories";
                var result = connection.Query<Category>(sql);
                return result.ToList();
            }
        }

        // Удаляем категорию по наименованию, flase в случае отстсвия значения к БД
        public bool Delete(string name)
        {
            using (var connection = new NpgsqlConnection(connectionString))
            {
                string sql = @"SELECT id FROM categories
                            WHERE name LIKE '" + name + "'";
                var result = connection.Query<Category>(sql);

                if (result == null)
                {
                    return false;
                }
                else
                {
                    sql = @"DELETE FROM categories WHERE name LIKE '" + name + "'";
                    result = connection.Query<Category>(sql);
                }
                return true;
            }
        }

        // Возвращаем id категории по наименованию
        public int GetCategoryId(string name)
        {
            using (var connection = new NpgsqlConnection(connectionString))
            {
                string sql = @"SELECT id FROM categories
                            WHERE name LIKE '" + name + "'";
                var result = connection.QueryFirstOrDefault<Category>(sql);
                if (result != null)
                {
                    return result.Id;
                }
                return -1;
            }
        }

        // Проверка существования категории по наименованию
        public bool isExists(string name)
        {
            using (var connection = new NpgsqlConnection(connectionString))
            {
                string sql = @"SELECT id FROM categories
                            WHERE name LIKE '" + name + "'";
                var result = connection.QueryFirstOrDefault<Category>(sql);
                if (result != null)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }

        // Переименование категории, flase, если категория отстствуем или новое наименование не уникально
        public bool Rename(string oldName, string newName)
        {
            using (var connection = new NpgsqlConnection(connectionString))
            {
                string sql = @"SELECT id FROM categories
                            WHERE name LIKE '" + oldName + "'";
                var result = connection.QueryFirstOrDefault<Category>(sql);
                if (result != null)
                {
                    try
                    {
                        sql = @"UPDATE categories
                            SET name = '" + newName + "' WHERE id = " + result.Id;
                        result = connection.QueryFirstOrDefault<Category>(sql);
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
    }
}
