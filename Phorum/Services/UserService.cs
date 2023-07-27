using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Phorum.AppConstants;
using Phorum.Entities;
using Phorum.Identity;
using Phorum.Models;
using Phorum.Repositories.UserRepository;

namespace Phorum.Services
{
    public class UserService : IUserService
    {
        private readonly IPasswordHasher<User> _passwordHasher;
        private readonly IUserRepository _userRepository;
        private readonly IMapper _autoMapper;
        private readonly IJwtProvider _jwtProvider;
        public UserService(IPasswordHasher<User> passwordHasher, IUserRepository userRepository, IMapper autoMapper, IJwtProvider jwtProvider)
        {
            _passwordHasher = passwordHasher;
            _userRepository = userRepository;
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
            _userRepository.CreateUser(user);
            _userRepository.SaveChanges();
        }

        public object Authenticate(UserLoginDTO model)
        {
            User? user = _userRepository.GetUser(model.Email);
            ArgumentNullException.ThrowIfNull(user);

            var validPassword = _passwordHasher.VerifyHashedPassword(user, user.Password, model.Password);

            if (validPassword == PasswordVerificationResult.Failed)
            {
                throw new Exception("wrong username or password");
            }

            var userDto = _autoMapper.Map<UserDTO>(user);

            AccessTokenDTO accessToken = _jwtProvider.GenerateJwtToken(user);
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
           RefreshToken? refreshToken = _userRepository.GetRefreshToken(token);
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
