using Shared.Interfaces;
using Shared.Models;

namespace DAL.Repositories
{
    public class ListUserRepository : IUserRepository
    {
        private static List<User> _users = new List<User>();

        // Выдаем роль пользоывателя по id
        public Role CheckUser(int id)
        {
            if (GetUser(id) != null)
            {
                return GetUser(id).Role;
            }
            else
            {
                Add(new User(id, Role.Customer));
                return GetUser(id).Role;
            }
        }

        // Меняем роль пользователя по id
        public void ChangeRole(int id, Role role)
        {
            if (GetUser(id) != null)
            {
                _users.Single(x => x.Id == id).Role = role;
            }            
        }

        // Выдаем список всех пользователей
        public List<User> GetAllUsers()
        {
            return _users;
        }

        // Добавляем нового пользователя
        public void Add(User user)
        {
            _users.Add(user);
        }

        // Добавляем нового пользователя или false, если существует
        public bool Add(int id, Role role)
        {
            if(GetUser(id) == null)
            {
                _users.Add(new User(id, role));
                return true;
            }
            else
            {
                return false;
            }
        }

        // Удаляем пользователя из репозитория
        public void Delete(int id)
        {
            var user = GetUser(id);
            if (user != null)
            {
                _users.Remove(user);
            }            
        }

        // Выдаем пользователя по его id
        private User GetUser(int id)
        {
            if (_users.Any(u => u.Id == id))
            {
                return _users.Single(u => u.Id == id);
            }
            else
            {
                return null;
            }
        }
    }
}
