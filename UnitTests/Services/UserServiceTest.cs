using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Phorum.Entities;
using Phorum.Identity;
using Phorum.Services;

namespace UnitTests.Services
{
    [TestClass]
    internal class UserServiceTest
    {
        private readonly UserService _userService;

        public UserServiceTest(IPasswordHasher<User> passwordHasher, PhorumContext phorumContext, IMapper autoMapper, IJwtProvider jwtProvider)
        {
            _userService = new UserService(passwordHasher, phorumContext, autoMapper, jwtProvider);
        }

        [TestMethod]
        public void test()
        {

        }

    }
}