using Phorum.Entities;
using Phorum.Models;

namespace Phorum.Repositories.PostRepository
{
    public interface IPostRepository
    {
        void SaveChanges();
        Post? GetPostById(int postId);
        IEnumerable<Post> GetAllPosts();
        void CreatePost(Post post);
        void UpdatePost(Post post);
    }
}
