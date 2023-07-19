using Phorum.Services;

namespace UnitTests.Services
{
    internal class UserServiceTest
    {
        private readonly UserService _userService;

        public UserServiceTest()
        {
            _userService = new UserService();
        }
    }
}
