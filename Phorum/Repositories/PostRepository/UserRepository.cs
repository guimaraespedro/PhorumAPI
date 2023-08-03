using Microsoft.EntityFrameworkCore;
using Phorum.Entities;


namespace Phorum.Repositories.PostRepository
{
    public class PostRepository : IPostRepository
    {
        private readonly PhorumContext _context;
        private bool disposed = false;
        public PostRepository(PhorumContext context) => _context = context;
        
        public void SaveChanges()
        {
            _context.SaveChanges();
        }

        public void CreatePost(Post post)
        {
            _context.Post.Add(post);
        }

        public void UpdatePost(Post post)
        {
            _context.Post.Update(post);
        }

        public Post? GetPostById(int postId)
        {

            Post? post = _context.Post.Include(p => p.User).Where(post => post.Id == postId).FirstOrDefault();

            return post;
        }

        public IEnumerable<Post> GetAllPosts()
        {
            IEnumerable<Post> posts = _context.Post.Include(p => p.User);

            return posts;
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
