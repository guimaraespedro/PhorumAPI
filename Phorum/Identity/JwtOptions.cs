namespace Phorum.Identity
{
    public class JwtOptions
    {
        public string JwtKey { get; set; }
        public string JwtIssuer { get; set; }
        public int JwtExpireInMinutes{ get; set; }
        public int RefreshTokenExpiresInDays { get; set; }
    }
}
