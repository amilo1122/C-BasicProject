using Dapper;
using Npgsql;
using Shared.Interfaces;
using Shared.Models;

namespace DAL.Repositories
{
    public class DapperUsersRepository : IUserRepository
    {
        // Создаем строку подключения к БД
        string connectionString = Config.SQLConnectionString;

        public bool Add(long id, Role role)
        {
            using (var connection = new NpgsqlConnection(connectionString))
            {
                try
                {
                    var sql = @"INSERT INTO Users (id, role) 
                                VALUES (" + id + ", '" + role + "');";
                    var result = connection.QueryFirstOrDefault<User>(sql);
                    return true;
                }
                catch
                {
                    return false;
                }
            }
        }

        // Меняем роль пользователя по id, false, если пользователь не найден
        public bool ChangeRole(long id, Role role)
        {
            using (var connection = new NpgsqlConnection(connectionString))
            {
                try
                {
                    var sql = @"UPDATE users
                            SET role = '" + role + "' WHERE id = " + id;
                    var result = connection.QueryFirstOrDefault<User>(sql);
                    return true;
                }
                catch
                {
                    return false;
                }
            }
        }

        // Возвращаем роль пользователя по id, если пользователь не найден - создаем нового с роль Customer
        public Role CheckUser(long id)
        {
            using (var connection = new NpgsqlConnection(connectionString))
            {
                var sql = @"SELECT role
                            FROM users
                            WHERE id = " + id;
                var result = connection.QueryFirstOrDefault<User>(sql);
                if (result == null)
                {
                    sql = @"INSERT INTO Users (id, role) 
                                VALUES (" + id + ", '" + Role.Customer + "');";
                    result = connection.QueryFirstOrDefault<User>(sql);
                    sql = @"SELECT role
                            FROM users
                            WHERE id = " + id;
                    result = connection.QueryFirstOrDefault<User>(sql);
                    return result.Role;
                }
                return result.Role;
            }
        }

        // Удаляем пользователя по id, false, если пользователь не найден
        public bool Delete(long id)
        {
            using (var connection = new NpgsqlConnection(connectionString))
            {
                try
                {
                    var sql = @"DELETE FROM users WHERE id = " + id;
                    var result = connection.QueryFirstOrDefault<User>(sql);
                    return true;
                }
                catch
                {
                    return false;
                }
            }
        }

        // Возвращаем список всех пользователей
        public List<User> GetAllUsers()
        {
            using (var connection = new NpgsqlConnection(connectionString))
            {
                string sql = @"SELECT id, role 
                            FROM users";
                var result = connection.Query<User>(sql);
                return result.ToList();
            }
        }
    }
}
