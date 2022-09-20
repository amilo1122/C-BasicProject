using Shared.Models;

namespace Shared.Interfaces
{    public interface ICategoryRepository
    {
        void Add(string name);
        void Add(string name, int id);
        bool Delete(string name);
        void Rename(string oldName, string newName);
        bool isExists(string name);
        List<Category> Browse();
        int GetCategoryId(string name);
        List<Category> GetCategoryList();
    }
}
