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
        private ILikeService _likeService;
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

    }
}