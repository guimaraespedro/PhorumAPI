
using Microsoft.EntityFrameworkCore;
using System.Text.Json.Serialization;

namespace Phorum.Entities;
    public class User
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;

        public string Email { get; set; } = string.Empty;
        [JsonIgnore]
        public string Password { get; set; }  = string.Empty;

        public DateTime DateOfBirth { get; set; }

        public int RoleId { get; set; }
        public Role Role { get; set; } = null!;

        public virtual List<Post> Posts { get; set; } = null!;
        public virtual List<Like> Likes { get; set; } = null!;
    }

