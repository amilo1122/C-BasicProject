namespace Shared.Models
{    
    public class Category
    {
        public string Name { get; set; }
        public int Id { get; set; }
        public Category(string name, int id)
        {
            Name = name;
            Id = id;
        }
    }
}
