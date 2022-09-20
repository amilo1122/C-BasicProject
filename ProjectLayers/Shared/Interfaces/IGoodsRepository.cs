using Shared.Models;

namespace Shared.Interfaces
{
    public interface IGoodsRepository
    {
        public void Add(int categoryId, string name, string description, decimal price, int quantity, string url);
        public void Add(int categoryId, string name, string description, int id, decimal price, int quantity, string url);
        public bool Delete(string name);
        public List<Good> GetAllGoods();
        public Good GetGood(int id);
        void ChangeQuantity(int id, int quantity);
        void ChangeCategoryId(int id, int categoryId);
        void ChangeName(int id, string name);
        void ChangeDescription(int id, string description);
        void ChangePrice(int id, decimal price);
        void ChangeGoodUrl(int id, string url);
    }
}
