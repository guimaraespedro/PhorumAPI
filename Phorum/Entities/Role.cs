using Microsoft.AspNetCore.Identity;

namespace Phorum.Entities
{
    public class Role
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;

    }
}
