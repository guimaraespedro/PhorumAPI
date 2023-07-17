using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Phorum.AppConstants;
using Phorum.Entities;
using Phorum.Identity;
using Phorum.Models;

namespace Phorum.Services
{
    public class UserService : IUserService
    {
        private readonly IPasswordHasher<User> _passwordHasher;
        private readonly PhorumContext _phorumContext;
        private readonly IMapper _autoMapper;
        private readonly IJwtProvider _jwtProvider;
        public UserService(IPasswordHasher<User> passwordHasher, PhorumContext phorumContext, IMapper autoMapper, IJwtProvider jwtProvider)
        {
            _passwordHasher = passwordHasher;
            _phorumContext = phorumContext;
            _autoMapper = autoMapper;
            _jwtProvider = jwtProvider;
        }

        public void RegisterUser(RegisterUserDTO model)
        {
            User user = new()
            {
                Name = model.Name,
                Email = model.Email,
                DateOfBirth = model.DateOfBirth,
                RoleId = (int)Roles.AvailableRoles.DefaultUser
            };
            var hashedPassword = _passwordHasher.HashPassword(user, model.Password);
            user.Password = hashedPassword;

            _phorumContext.User.Add(user);
            _phorumContext.SaveChanges();
        }

        public object Authenticate(UserLoginDTO model)
        {
            User? user = _phorumContext.User
              .Include(user => user.Role)
              .FirstOrDefault(user => user.Email == model.Email);
            ArgumentNullException.ThrowIfNull(user);

            var validPassword = _passwordHasher.VerifyHashedPassword(user, user.Password, model.Password);

            if (validPassword == PasswordVerificationResult.Failed)
            {
                throw new Exception("wrong username or password");
            }

            var userDto = _autoMapper.Map<UserDTO>(user);

            string accessToken = _jwtProvider.GenerateJwtToken(user);
            string refreshToken = _jwtProvider.GenerateRefreshToken(user);
            TokenDTO tokenDTO = new ()
            {
                AccessToken = accessToken,
                RefreshToken = refreshToken,
            };

            return new { user = userDto, tokenDTO };
        }

        public TokenDTO RefreshToken(string token)
        {
           RefreshToken? refreshToken = _phorumContext.RefreshToken.Include(t => t.User).Include(t=> t.User.Role).FirstOrDefault(t => t.TokenId == token && !t.IsBlackListed);
           TokenDTO tokens = new();

           if(refreshToken == null)
           {
                ArgumentNullException.ThrowIfNull(refreshToken);
           }

            var accessToken = _jwtProvider.GenerateJwtToken(refreshToken.User);
            tokens.AccessToken = accessToken;

           if(refreshToken.ExpirationDate < DateTime.Now)
           {
                string newRefreshToken = _jwtProvider.GenerateRefreshToken(refreshToken.User);
                tokens.RefreshToken = newRefreshToken;
                return tokens;
           }
            tokens.RefreshToken = refreshToken.TokenId;
            return tokens;
        }

    }
}
