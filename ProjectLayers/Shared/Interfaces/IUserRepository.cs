using Shared.Models;

namespace Shared.Interfaces
{    public interface IUserRepository
    {
        void Add(User user);
        bool Add(long id, Role role);
        void Delete(long id);
        void ChangeRole(long id, Role role);
        Role CheckUser(long id);
        List<User> GetAllUsers();
    }
}
