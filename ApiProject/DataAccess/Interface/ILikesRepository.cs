using ApiProject.Data.CustomModels;
using ApiProject.Entities;
using ApiProject.Helpers;

namespace ApiProject.DataAccess.Interface{
    public interface ILikesRepository
    {
         Task<UserLike> GetUserLike(int SourceUserId, int TargetUserId);
         Task<AppUser> GetUserWithLikes(int userId);
         Task<PageList<LikeDto>> GetUserLikes (LikesParams likesParams);
    }
}