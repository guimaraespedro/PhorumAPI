using Phorum.Entities;
using Phorum.Models;

namespace Phorum.Identity
{
    public interface IJwtProvider
    {
        public AccessTokenDTO GenerateJwtToken(User user);
        public string GenerateRefreshToken(User user);
    }
}
