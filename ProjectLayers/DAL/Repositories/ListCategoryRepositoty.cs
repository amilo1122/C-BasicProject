using DAL.Generators;
using Shared.Interfaces;
using Shared.Models;

namespace DAL.Repositories
{
    public class ListCategoryRepositoty : ICategoryRepository
    {
        private static List<Category> _category = new List<Category>();
        BaseRepository categoryIndex = new BaseRepository();

        public void Add(string name)
        {
            if (!_category.Any(u => u.Name == name) && name != null && name != "")
            {
                _category.Add(new Category(name, categoryIndex.IncrementCategoryIndex()));
            }
        }

        public void Add(string name, int id)
        {
            if (!_category.Any(u => u.Name == name) && name != null && name != "")
            {
                _category.Add(new Category(name, id));
            }
        }

        public List<Category> Browse()
        {
            return _category;
        }

        // Удаляем категорию
        public bool Delete(string name)
        {
            if (_category.Any(u => u.Name == name))
            {
                var category = _category.Single(x => x.Name == name);
                var index = _category.IndexOf(category);
                _category.RemoveAt(index);
                return true;
            }
            else
            {
                return false;
            }
        }

        // Задаем новое имя категории
        public void Rename(string oldName, string newName)
        {
            var category = _category.Single(x => x.Name == oldName);
            var index = _category.IndexOf(category);
            _category[index].Name = newName;
        }

        // Прооверяем существование категории
        public bool isExists(string name)
        {
            if (_category.Any(u => u.Name == name))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        // Возвращаем имя категории по id
        public int GetCategoryId(string name)
        {
            var category = _category.Single(x => x.Name == name);
            return category.Id;
        }

        public List<Category> GetCategoryList()
        {
            return _category;
        }
    }
}
