using Microsoft.EntityFrameworkCore;
using Phorum.Entities;


namespace Phorum.Repositories.PostRepository
{
    public class LikeRepository : ILikeRepository
    {
        private readonly PhorumContext _context;
        private bool disposed = false;
        public LikeRepository(PhorumContext context) => _context = context;
        
        public void SaveChanges()
        {
            _context.SaveChanges();
        }

        public void  CreateLike(Like like)
        {
            _context.Like.Add(like);
        }


        public Like? GetLikeByPostAndUserId(int postId, int userId)
        {
            Like? like = _context.Like.Where(like => like.PostId == postId && like.UserId == userId).FirstOrDefault();
            return like;
        }

        public void DeleteLike(Like like)
        {
            _context.Like.Remove(like);
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
