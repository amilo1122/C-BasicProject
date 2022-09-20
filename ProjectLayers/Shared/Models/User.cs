namespace Shared.Models
{    public class User
    {
        public int Id { get; set; }
        public Role Role { get; set; }

        public User(int id, Role role)
        {
            Id = id;
            Role = role;
        }
    }
}
