using Microsoft.EntityFrameworkCore;
using Phorum.Entities;

namespace Phorum.Repositories.UserRepository
{
    public class UserRepository : IUserRepository
    {
        private readonly PhorumContext _context;
        private bool disposed = false;
        public UserRepository(PhorumContext context) => _context = context;
        
        public void SaveChanges()
        {
            _context.SaveChanges();
        }

        public void CreateUser(User user)
        {
            _context.User.Add(user);
        }

        public User? GetUser(string email)
        {

           User? user = _context.User
              .Include(user => user.Role)
              .FirstOrDefault(user => user.Email == email);

            return user;
        }

        public RefreshToken? GetRefreshToken(string token)
        {
            RefreshToken? refreshToken = _context.RefreshToken.Include(t => t.User).Include(t => t.User.Role).FirstOrDefault(t => t.TokenId == token && !t.IsBlackListed);
            return refreshToken;
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    _context.Dispose();
                }
            }
            this.disposed = true;
        }
    }
}
