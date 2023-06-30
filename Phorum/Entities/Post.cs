using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace Phorum.Entities
{
    public class Post
    {
        public int Id { get; set; }
        public string Content { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public int UserId { get; set; }
        public virtual User User { get; set; } = null!;
        public virtual List<Like> Likes { get; set; } = null!;
    }
}
;