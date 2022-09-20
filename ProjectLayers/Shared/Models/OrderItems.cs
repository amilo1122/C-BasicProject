
namespace Shared.Models
{
    public class OrderItems
    {
        public int Id { get; set; }
        public int OrderId { get; set; }
        public int GoodId { get; set; }
        public decimal Price { get; set; }
        public int Quantity { get; set; }
    }
}
