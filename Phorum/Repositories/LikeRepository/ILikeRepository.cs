using Phorum.Entities;
using Phorum.Models;

namespace Phorum.Repositories.PostRepository
{
    public interface ILikeRepository
    {
        void SaveChanges();
        Like? GetLikeByPostAndUserId(int postId, int userId);
        void CreateLike(Like like);
        void DeleteLike(Like like);
    }
}
