using Shared.Models;

namespace Shared.Interfaces
{    public interface IUserRepository
    {
        void Add(User user);
        bool Add(int id, Role role);
        void Delete(int id);
        void ChangeRole(int id, Role role);
        Role CheckUser(int id);
        List<User> GetAllUsers();
    }
}
