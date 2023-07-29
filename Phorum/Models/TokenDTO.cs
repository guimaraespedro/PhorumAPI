using Phorum.Entities;

namespace Phorum.Models
{
    public class TokenDTO
    {
        public AccessTokenDTO AccessToken { get; set; }
        public string RefreshToken { get; set; }

    }

    public class AuthDTO
    {
        public AccessTokenDTO AccessToken { get; set; }
        public string RefreshToken { get; set; }

        public UserDTO UserDTO { get; set; }
    }

    public class AccessTokenDTO
    {
        public string Token { get; set; }
        public DateTime ExpiresIn { get; set; }
    }
}