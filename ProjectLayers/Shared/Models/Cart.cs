
namespace Shared.Models
{
    public class Cart
    {
        public int Id { get; set; }
        public long UserId { get; set; } 
        public int GoodId { get; set; }
        public int Quantity { get; set; }

        public Cart(int id, long userId, int goodsId, int quantity)
        {
            Id = id;
            UserId = userId;
            GoodId = goodsId;
            Quantity = quantity;
        }
    }
}
