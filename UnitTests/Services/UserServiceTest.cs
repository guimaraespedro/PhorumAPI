using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Phorum.Entities;
using Phorum.Identity;
using Phorum.Models;
using Phorum.Repositories.UserRepository;
using Phorum.Services;
using System.Reflection.Metadata;

namespace UnitTests.Services
{
    [TestClass]
    public class UserServiceTest
    {
        private IUserService _userService;
        private Mock<IUserRepository> _userRepositoryMock;
        private Mock<IPasswordHasher<User>> _passwordHasherMock;
        private Mock<IJwtProvider> _jwtProviderMock;

        [TestInitialize]
        public void TestSetup()
        {
            _passwordHasherMock = new Mock<IPasswordHasher<User>>();
            _userRepositoryMock = new Mock<IUserRepository>();
            _jwtProviderMock = new Mock<IJwtProvider>();
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<User, UserDTO>();
            });

            var mapper = config.CreateMapper();
            _userService = new UserService(_passwordHasherMock.Object, _userRepositoryMock.Object, mapper, _jwtProviderMock.Object);
        }

        [TestMethod]
        public void RegisterUser_should_create_user_and_save_changes()
        {
            // Arrange
            RegisterUserDTO registerUserDTO = new()
            {
                DateOfBirth = DateTime.Now,
                Email = "placeholder@email.com",
                Name = "Jhon doe",
                Password = "123",
                
            };

            _userRepositoryMock.Setup(repo => repo.CreateUser(It.IsAny<User>()));
            _userRepositoryMock.Setup(repo => repo.SaveChanges());
            // Act
            _userService.RegisterUser(registerUserDTO);
            // Assert
            _userRepositoryMock.Verify(repo => repo.CreateUser(It.IsAny<User>()), Times.Once());
            _userRepositoryMock.Verify(repo => repo.SaveChanges(), Times.Once());
        }

        [TestMethod]
        public void AuthenticateUser_should_create_accessToken_and_refreshToken()
        {
            UserLoginDTO userLoginDTO = new()
            {
                Email = "JhonDoe@gmail.com",
                Password = "123"
            };
            DateTime expiresIn = DateTime.Now;
            AccessTokenDTO accessTokenDTO = new()
            {
                ExpiresIn = expiresIn,
                Token = "accessToken"
            };
            User user = new User()
            {
                Id = 1,
                Email = "JhonDoe@gmail.com",
                Password = "123",
                DateOfBirth = DateTime.Now,
                Name = "Guima"
            };
             _jwtProviderMock.Setup(jwtProvider => jwtProvider.GenerateJwtToken(It.IsAny<User>())).Returns(accessTokenDTO);
             _jwtProviderMock.Setup(jwtProvider => jwtProvider.GenerateRefreshToken(It.IsAny<User>())).Returns("refreshToken");
            _passwordHasherMock.Setup(hasher => hasher.VerifyHashedPassword(It.IsAny<User>(), It.IsAny<string>(), It.IsAny<string>())).Returns(PasswordVerificationResult.Success);
            _userRepositoryMock.Setup(repo => repo.GetUser(userLoginDTO.Email)).Returns(user);
            // Act
            AuthDTO result = _userService.Authenticate(userLoginDTO);
            // Assert
            Assert.AreEqual(user.Email, result.UserDTO.Email);
            Assert.AreEqual(result.AccessToken.ExpiresIn, expiresIn);
            Assert.IsNotNull(result.RefreshToken);
            Assert.IsNotNull(result.AccessToken.Token);
        }

        [TestMethod]
        public void RefreshToken_should_return_new_access_and_refresh_tokens()
        {
            string refreshToken = "refreshToken";
            DateTime expiresIn = DateTime.Now;
            AccessTokenDTO accessTokenDTO = new()
            {
                ExpiresIn = expiresIn,
                Token = "accessToken"
            };
            RefreshToken refreshTokenEntity = new()
            {
                TokenId = refreshToken,
                ExpirationDate = expiresIn.AddDays(2),
                IsBlackListed = false,
                UserId = 1
            };
            _jwtProviderMock.Setup(jwtProvider => jwtProvider.GenerateJwtToken(It.IsAny<User>())).Returns(accessTokenDTO);
            _jwtProviderMock.Setup(jwtProvider => jwtProvider.GenerateRefreshToken(It.IsAny<User>())).Returns("refreshToken");
            _userRepositoryMock.Setup(repo => repo.GetRefreshToken(refreshToken)).Returns(refreshTokenEntity);
            // Act
            TokenDTO result = _userService.RefreshToken(refreshToken);
            // Assert
            Assert.AreEqual(result.RefreshToken, refreshToken);
            Assert.AreEqual(result.AccessToken.ExpiresIn, expiresIn);
            Assert.IsNotNull(result.RefreshToken);
            Assert.IsNotNull(result.AccessToken.Token);
        }

    }
}