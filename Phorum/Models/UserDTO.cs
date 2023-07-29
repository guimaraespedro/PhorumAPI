using Phorum.Entities;

namespace Phorum.Models
{
    public class UserDTO
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;

        public string Email { get; set; }
        public DateTime DateOfBirth { get; set; }

        public Role Role { get; set; } = null!;

    }
}