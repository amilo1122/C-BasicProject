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

        public void Add(long userId, int goodsId, int quantity)
        {
            var index = cartIndex.IncrementCartIndex();
            Cart cart = new Cart(index, userId, goodsId, quantity);
            _cart.Add(cart);
        }
        public void Add(long userId, int goodsId, int quantity, int id)
        {
            Cart cart = new Cart(id, userId, goodsId, quantity);
            _cart.Add(cart);
        }

        public List<Cart> GetAllCart()
        {
            return _cart;
        }

        public List<Cart> GetUserCart(long userId)
        {
            var userCart = _cart.Where(x => x.UserId == userId).ToList();
            if (userCart != null)
            {
                return userCart;
            }
            else
            {
                return null;
            }
        }

        public void UpdateQuantity(long userId, int goodsId, int quantity)
        {
            var currentCart = _cart.Single(x => x.UserId == userId && x.GoodsId == goodsId);
            var currentQuantity = currentCart.Quantity;
            if (quantity == 1)
            {
                currentQuantity++;
            }
            else if (quantity == -1)
            {
                if (currentQuantity == 1)
                {
                    _cart.Remove(currentCart);
                }
                else
                {
                    currentQuantity--;
                }                
            }            
        }

        public void Remove(long userId, int goodsId)
        {
            var cart = _cart.Single(x => x.UserId == userId && x.GoodsId == goodsId);
            _cart.Remove(cart);
        }
    }
}
