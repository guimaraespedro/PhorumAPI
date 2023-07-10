using Phorum.Models;

namespace Phorum.Services
{
    public interface IUserService
    {
        public void RegisterUser(RegisterUserDTO model);
        public object Authenticate(UserLoginDTO model);
    }
}
