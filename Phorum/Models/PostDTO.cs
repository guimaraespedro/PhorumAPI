using System.ComponentModel.DataAnnotations;

namespace Phorum.Models
{
    public class PostDTO
    {
        public int Id { get; set; } 
        public string Content { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
        public UserDTO User { get; set; } = null!;

    }
}
