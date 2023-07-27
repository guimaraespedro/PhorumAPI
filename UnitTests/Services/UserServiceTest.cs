using AutoMapper;
using Microsoft.AspNetCore.Identity;
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

        [TestInitialize]
        public void TestSetup()
        {
            _passwordHasherMock = new Mock<IPasswordHasher<User>>();
            _userRepositoryMock = new Mock<IUserRepository>();
            var mapperMock = new Mock<IMapper>();
            var jwtProviderMock = new Mock<IJwtProvider>();
            _userService = new UserService(_passwordHasherMock.Object, _userRepositoryMock.Object, mapperMock.Object, jwtProviderMock.Object);
        }

        [TestMethod]
        public void RegisterUser_should_create_user_and_update_savechanges()
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


    }
}