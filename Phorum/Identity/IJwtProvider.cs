using Phorum.Entities;

namespace Phorum.Identity
{
    public interface IJwtProvider
    {
        public string GenerateJwtToken(User user);
        public string GenerateRefreshToken(User user);
    }
}
