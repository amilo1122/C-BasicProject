namespace Shared.Models
{
    public class Order
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public decimal TotalSum { get; set; }
        public DateTime CreatedDate { get; set; }    
    }
}
