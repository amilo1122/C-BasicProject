using Shared.Interfaces;
using Shared.Models;

namespace DAL.Repositories
{
    public class ListUserRepository : IUserRepository
    {
        private static List<User> _users = new List<User>();

        // Выдаем роль пользоывателя по id
        public Role CheckUser(long id)
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
        public void ChangeRole(long id, Role role)
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
        public bool Add(long id, Role role)
        {
            if(GetUser(id) == null)
            {
                User user = new User(id, role);
                _users.Add(user);
                return true;
            }
            else
            {
                return false;
            }
        }

        // Удаляем пользователя из репозитория
        public void Delete(long id)
        {
            var user = GetUser(id);
            if (user != null)
            {
                _users.Remove(user);
            }            
        }

        // Выдаем пользователя по его id
        private User GetUser(long id)
        {
            var flag = _users.Any(u => u.Id == id);
            if (flag)
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
