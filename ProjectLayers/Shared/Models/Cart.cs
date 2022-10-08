
namespace Shared.Models
{
    public class Cart
    {
        public int Id { get; set; }
        public long UserId { get; set; } 
        public int GoodId { get; set; }
        public int Quantity { get; set; }
    }
}
