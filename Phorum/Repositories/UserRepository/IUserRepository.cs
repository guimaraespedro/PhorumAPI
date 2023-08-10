using Phorum.Entities;

namespace Phorum.Repositories.UserRepository
{
    public interface IUserRepository
    {
        void SaveChanges();
        void CreateUser(User user);
        User? GetUser(string email);
        User? GetUser(int id);
        RefreshToken? GetRefreshToken(string token);
    }
}
