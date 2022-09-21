using DAL.Generators;
using Shared.Interfaces;
using Shared.Models;

namespace DAL.Repositories
{
    public class ListGoodsRepository : IGoodsRepository
    {
        private static List<Good> _goods = new List<Good>();
        BaseRepository goodIndex = new BaseRepository();

        // Добавляем новый товар в репозиторий
        public void Add(int categoryId, string name, string description, decimal price, int quantity, string url)
        {
            if (!_goods.Any(u => u.Name == name) && name != null && name != "")
            {
                var index = goodIndex.IncrementGoodIndex();
                Good good = new Good(categoryId, name, description, index, price, quantity, url);
                _goods.Add(good);
            }
        }

        // Добавляем новый товар в репозиторий из файла
        public void Add(int categoryId, string name, string description, int id, decimal price, int quantity, string url)
        {
            if (!_goods.Any(u => u.Name == name) && name != null && name != "")
            {
                Good good = new Good(categoryId, name, description, id, price, quantity, url);
                _goods.Add(good);
            }
        }

        // Удаляем товар по его id из репозитория
        public bool Delete(string name)
        {
            if (_goods.Any(u => u.Name == name))
            {
                var good = _goods.Single(x => x.Name == name);
                var index = _goods.IndexOf(good);
                _goods.RemoveAt(index);
                return true;
            }
            else
            {
                return false;
            }
        }

        // Выводим список всех товаров репозитория
        public List<Good> GetAllGoods()
        {
            return _goods;
        }

        // Возвращаем товар по его id
        public Good GetGood(int id)
        {
            return _goods[id];
        }

        // Изменяем доступное количество
        public void ChangeQuantity(int id, int quantity)
        {
            _goods.Single(x => x.Id == id).Quantity = quantity;
        }

        // Меняем категорию товара
        public void ChangeCategoryId(int id, int categoryId)
        {
            _goods.Single(x => x.Id == id).CategoryId = categoryId;
        }

        // Меняем наименование товара
        public void ChangeName(int id, string name)
        {
            _goods.Single(x => x.Id == id).Name = name;
        }

        // Меняем описание товара
        public void ChangeDescription(int id, string description)
        {
            _goods.Single(x => x.Id == id).Description = description;
        }

        // Меняем стоимость товара
        public void ChangePrice(int id, decimal price)
        {
            _goods.Single(x => x.Id == id).Price = price;
        }

        // Меняем ссылку на изображение товара
        public void ChangeGoodUrl(int id, string url)
        {
            _goods.Single(x => x.Id == id).Url = url;
        }
    }
}
