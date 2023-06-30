﻿using Microsoft.IdentityModel.Tokens;
using Phorum.Entities;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Phorum.Identity
{
    public class JwtProvider : IJwtProvider
    {
       private readonly IConfiguration _configuration;

        public JwtProvider(IConfiguration configuration)
        {
            _configuration = configuration;
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
            var expires = DateTime.Now.AddDays(_jwtOptions.JwtExpireDays);

            var token = new JwtSecurityToken(_jwtOptions.JwtIssuer, _jwtOptions.JwtIssuer, claims, expires: expires, signingCredentials: credentials);

            return new JwtSecurityTokenHandler().WriteToken(token);

        }
    }
}
