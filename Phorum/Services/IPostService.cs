using Phorum.Models;

namespace Phorum.Services
{
    public interface IPostService
    {
        public void CreatePost(string content);

        public ICollection<PostDTO> GetAllPosts();

        public PostDTO GetPostById(int postId);

        public void UpdatePost(PostDTO model, int postId);

    }
}
