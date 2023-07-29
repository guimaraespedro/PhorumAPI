using Phorum.Models;

namespace Phorum.Services
{
    public interface IUserService
    {
        public void RegisterUser(RegisterUserDTO model);
        public AuthDTO Authenticate(UserLoginDTO model);

        public TokenDTO RefreshToken(string refreshToken);

    }
}
