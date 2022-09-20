namespace Shared.Models
{    public class Good
    {
        public int CategoryId { get; set; }
        public string Name { set; get; }
        public string Description { set; get; }
        public int Id { set; get; }
        public decimal Price { set; get; }
        public int Quantity { set; get; }
        public string Url { set; get; }

        public Good()
        {

        }

        public Good(int categoryId, string name, string description, int id, decimal price, int quantity, string url)
        {
            CategoryId = categoryId;
            Name = name;
            Description = description;
            Id = id;
            Price = price;
            Quantity = quantity;
            Url = url;
        }
    }
}
