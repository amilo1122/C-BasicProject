using Dapper;
using Npgsql;
using Shared.Interfaces;
using Shared.Models;

namespace DAL.Repositories
{
    public class DapperCategoryRepository : ICategoryRepository
    {
        string connectionString = Config.SQLConnectionString;
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

        public bool Delete(string name)
        {
            using (var connection = new NpgsqlConnection(connectionString))
            {
                string sql = @"SELECT id FROM categories
                            WHERE name LIKE '%" + name + "%'";
                var result = connection.Query<Category>(sql);

                if (result == null)
                {
                    return false;
                }
                else
                {
                    sql = @"DELETE FROM categories WHERE name LIKE '%" + name + "%'";
                    result = connection.Query<Category>(sql);
                }
                return true;
            }
        }

        public int GetCategoryId(string name)
        {
            using (var connection = new NpgsqlConnection(connectionString))
            {
                string sql = @"SELECT id FROM categories
                            WHERE name LIKE '%" + name + "%'";
                var result = connection.QueryFirstOrDefault<Category>(sql);
                if (result != null)
                {
                    return result.Id;
                }
                return -1;
            }
        }

        public bool isExists(string name)
        {
            using (var connection = new NpgsqlConnection(connectionString))
            {
                string sql = @"SELECT id FROM categories
                            WHERE name LIKE '%" + name + "%'";
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

        public bool Rename(string oldName, string newName)
        {
            using (var connection = new NpgsqlConnection(Config.SQLConnectionString))
            {
                string sql = @"SELECT id FROM categories
                            WHERE name LIKE '%" + oldName + "%'";
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
