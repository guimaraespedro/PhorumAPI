using Microsoft.IdentityModel.Tokens;
using Phorum.Entities;
using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace Phorum.Identity
{
    public class JwtProvider : IJwtProvider
    {
        private readonly IConfiguration _configuration;
        private readonly PhorumContext _phorumContext;

        public JwtProvider(IConfiguration configuration, PhorumContext phorumContext)
        {
            _configuration = configuration;
            _phorumContext = phorumContext; 
        }

        public string GenerateJwtToken(User user)
        {
            JwtOptions _jwtOptions = new JwtOptions();
            _configuration.GetSection("jwt").Bind(_jwtOptions);

            List<Claim> claims = new()
            {
                new Claim(ClaimTypes.Name, user.Name),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim("UserId" ,user.Id.ToString()),
                new Claim(ClaimTypes.Role, user.Role.Name)
            };

            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtOptions.JwtKey));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
            var expires = DateTime.Now.AddMinutes(_jwtOptions.JwtExpireInMinutes);

            var token = new JwtSecurityToken(_jwtOptions.JwtIssuer, _jwtOptions.JwtIssuer, claims, expires: expires, signingCredentials: credentials);

            return new JwtSecurityTokenHandler().WriteToken(token);

        }

        public string GenerateRefreshToken(User user)
        {
            JwtOptions _jwtOptions = new();
            _configuration.GetSection("jwt").Bind(_jwtOptions);

            DateTime refreshTokenExpiration = DateTime.Now.AddDays(_jwtOptions.RefreshTokenExpiresInDays);
            byte[] randomBytes = new byte[32];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(randomBytes);
            }

            string token = Convert.ToBase64String(randomBytes);

            RefreshToken refreshToken = new()
            {
                ExpirationDate = refreshTokenExpiration,
                IsBlackListed = false,
                TokenId = token,
                User = user,
                UserId = user.Id
            };

            _phorumContext.RefreshToken.Add(refreshToken);
            _phorumContext.SaveChanges();

            return token;
        }
    }
}
