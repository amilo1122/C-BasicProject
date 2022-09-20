
namespace Shared.Models
{
    public class Cart
    {
        public int Id { get; set; }
        public int UserId { get; set; } 
        public int GoodsId { get; set; }
        public int Quantity { get; set; }

        public Cart(int id, int userId, int goodsId, int quantity)
        {
            Id = id;
            UserId = userId;
            GoodsId = goodsId;
            Quantity = quantity;
        }
    }
}
