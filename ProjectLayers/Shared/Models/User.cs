namespace Shared.Models
{    public class User
    {
        public long Id { get; set; }
        public Role Role { get; set; }

        public User(long id, Role role)
        {
            Id = id;
            Role = role;
        }
    }
}
