using Phorum.Entities;
using Phorum.Helpers;
using Phorum.Repositories.PostRepository;

namespace Phorum.Services
{
    public class LikeService : ILikeService
    {
        private readonly IPostRepository _postRepository;
        private readonly ILikeRepository _likeRepository;
        private readonly IHttpContextHelper _httpContextHelper;
        public LikeService(IPostRepository postRepository, ILikeRepository likeRepository, IHttpContextHelper httpContextHelper)
        {
            _postRepository = postRepository;
            _likeRepository = likeRepository;
            _httpContextHelper = httpContextHelper; 
        }

        public void LikePost(int postId)
        {
            Post? post = _postRepository.GetPostById(postId);

            ArgumentNullException.ThrowIfNull(post);

            int userId = _httpContextHelper.GetUserId();
            Like? alreadyLiked = _likeRepository.GetLikeByPostAndUserId(postId, userId);
            if (alreadyLiked != null)
            {
                throw new Exception("Post already liked");
            }
            Like like = new()
            {
                PostId = postId,
                UserId = userId,
            };

            _likeRepository.CreateLike(like);
            _likeRepository.SaveChanges();
        }

        public void DeleteLike(int postId)
        {
            Post? post = _postRepository.GetPostById(postId);
            ArgumentNullException.ThrowIfNull(post, nameof(post));
            int userId = _httpContextHelper.GetUserId();
            Like? like = _likeRepository.GetLikeByPostAndUserId(postId, userId);

            ArgumentNullException.ThrowIfNull(like, nameof(like));

            _likeRepository.DeleteLike(like);
            _likeRepository.SaveChanges();
        }
    }
}
