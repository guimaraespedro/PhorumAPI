namespace Phorum.Services
{
    public interface ILikeService
    {
        public void LikePost(int postId);
        public void DeleteLike(int postId);

    }
}
