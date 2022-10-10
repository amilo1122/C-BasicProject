using Shared.Models;

namespace Shared.Interfaces
{    public interface ICategoriesRepository
    {
        bool Add(string name);
        bool Delete(string name);
        bool Rename(string oldName, string newName);
        bool isExists(string name);
        List<Category> Browse();
        int GetCategoryId(string name);
    }
}
