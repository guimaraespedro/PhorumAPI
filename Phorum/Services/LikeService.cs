using Phorum.Entities;
using Phorum.Helpers;

namespace Phorum.Services
{
    public class LikeService : ILikeService
    {
        private readonly PhorumContext _phorumContext;
        private readonly HttpContextHelper _httpContextHelper;
        public LikeService(PhorumContext phorumContext, HttpContextHelper httpContextHelper)
        {
            _phorumContext = phorumContext;
            _httpContextHelper = httpContextHelper; 
        }

        public void LikePost(int postId)
        {
            bool post = _phorumContext.Post.Any(post => post.Id == postId);
            if (!post)
            {
                throw new ArgumentNullException(nameof(postId), "post not found");
            }
            int userId = _httpContextHelper.GetUserId();
            Like like = new()
            {
                PostId = postId,
                UserId = userId,
            };

            _phorumContext.Like.Add(like);
            _phorumContext.SaveChanges();
        }

        public void DeleteLike(int postId)
        {
            Post? post = _phorumContext.Post.FirstOrDefault(post => post.Id == postId);
            ArgumentNullException.ThrowIfNull(post, nameof(post));

            Like? like = _phorumContext.Like.FirstOrDefault(like => like.PostId == postId);

            ArgumentNullException.ThrowIfNull(like, nameof(like));

            _phorumContext.Remove(like);
            _phorumContext.SaveChanges();
        }
    }
}
