using ApiProject.Data.AppContextFile;
using ApiProject.Data.CustomModels;
using ApiProject.Entities;
using ApiProject.Helpers;
using Microsoft.EntityFrameworkCore;

namespace ApiProject.DataAccess.Interface{
    public class LikesRepository : ILikesRepository
    {
        private readonly DatingAppContext _Context;
        public LikesRepository(DatingAppContext dbcontext){
            this._Context = dbcontext;
        }
        public async Task<UserLike> GetUserLike(int SourceUserId, int TargetUserId)
        {
            return await _Context.Likes.FindAsync(SourceUserId, TargetUserId);
        }

        public async Task<PageList<LikeDto>> GetUserLikes(LikesParams likesParams)
        {
            // creating iQueryable for users and likes table.
            var users = _Context.appUser.OrderBy(u=>u.UserName).AsQueryable();
            var likes = _Context.Likes.AsQueryable();

            if(likesParams.Predicate == "liked"){
                likes = likes.Where(like => like.SourceUserId == likesParams.UserId);
                users = likes.Select(like => like.TargetUser);
            }

            if(likesParams.Predicate == "likedBy"){
                likes = likes.Where(like => like.TargetUserId == likesParams.UserId);
                users = likes.Select(like => like.SourceUser);
            }

            var likedUsers =  users.Select(user => new LikeDto{
                UserName = user.UserName,
                KnownAs = user.knownAs,
                Age = 1,
                PhotoUrl = user.Photos.FirstOrDefault(x=>x.IsMain).Url,
                City = user.City,
                Id = user.Id
            });

            return await PageList<LikeDto>.CreateAsync(likedUsers,likesParams.PageNumber,likesParams.PageSize);
        
        }

        public async Task<AppUser> GetUserWithLikes(int userId)
        {
            return await _Context.appUser
            .Include(x=>x.LikedUsers)
            .FirstOrDefaultAsync(x=>x.Id == userId);
        }
    }
}