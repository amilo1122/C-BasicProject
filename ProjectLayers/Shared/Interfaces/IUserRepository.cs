using Shared.Models;

namespace Shared.Interfaces
{    public interface IUserRepository
    {
        bool Add(long id, Role role);
        bool Delete(long id);
        bool ChangeRole(long id, Role role);
        Role CheckUser(long id);
        List<User> GetAllUsers();
    }
}
