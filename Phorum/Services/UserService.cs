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
        private readonly ILogger _logger;
        private readonly IMapper _autoMapper;
        private readonly IJwtProvider _jwtProvider;
        public UserService(IPasswordHasher<User> passwordHasher, PhorumContext phorumContext,ILogger logger, IMapper autoMapper, IJwtProvider jwtProvider)
        {
            _passwordHasher = passwordHasher;
            _phorumContext = phorumContext;
            _logger = logger;
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

            var token = _jwtProvider.GenerateJwtToken(user);

            return new{ user = userDto, token};
        }

    }
}
