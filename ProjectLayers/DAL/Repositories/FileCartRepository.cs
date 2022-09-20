using Shared.Interfaces;
using Shared.Models;
using DAL.FileStores;
using DAL.Generators;

namespace DAL.Repositories
{
    public class FileCartRepository : ICartRepository
    {
        private List<Cart> _cart = new List<Cart>();
        BaseRepository cartIndex = new BaseRepository();

        public void Add(int userId, int goodsId, int quantity)
        {
            _cart.Add(new Cart(cartIndex.IncrementCartIndex(), userId, goodsId, quantity));
        }
        public void Add(int userId, int goodsId, int quantity, int id)
        {
            _cart.Add(new Cart(id, userId, goodsId, quantity));
        }

        public List<Cart> GetAllCart()
        {
            return _cart;
        }

        public List<Cart> GetUserCart(int userId)
        {
            if (_cart.Select(x => x.UserId == userId).ToList() != null)
            {
                return _cart.Where(x => x.UserId == userId).ToList();
            }
            else
            {
                return null;
            }
        }

        public void UpdateQuantity(int userId, int goodsId, int quantity)
        {
            int currentQuantity = _cart.Single(x => x.UserId == userId && x.GoodsId == goodsId).Quantity;
            if (quantity == 1)
            {
                _cart.Single(x => x.UserId == userId && x.GoodsId == goodsId).Quantity++;
            }
            else if (quantity == -1)
            {
                if (currentQuantity == 1)
                {
                    _cart.Remove(_cart.Single(x => x.UserId == userId && x.GoodsId == goodsId));
                }
                else
                {
                    _cart.Single(x => x.UserId == userId && x.GoodsId == goodsId).Quantity--;
                }                
            }            
        }

        public void Remove(int userId, int goodsId)
        {
            _cart.Remove(_cart.Single(x => x.UserId == userId && x.GoodsId == goodsId));
        }
    }
}
