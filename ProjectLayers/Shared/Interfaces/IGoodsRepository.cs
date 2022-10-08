using Shared.Models;

namespace Shared.Interfaces
{
    public interface IGoodsRepository
    {
        public bool Add(int categoryId, string name, string description, decimal price, int quantity, string url);
        public bool Delete(string name);
        public List<Good> GetAllGoods();
        public Good GetGood(int id);
        bool ChangeQuantity(int id, int quantity);
        bool ChangeCategoryId(int id, int categoryId);
        bool ChangeName(int id, string name);
        bool ChangeDescription(int id, string description);
        bool ChangePrice(int id, decimal price);
        bool ChangeGoodUrl(int id, string url);
    }
}
