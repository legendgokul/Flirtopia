using ApiProject.Data.CustomModels;
using ApiProject.DataAccess.Interface;
using ApiProject.Entities;
using ApiProject.Extensions;
using ApiProject.Extensions.LibraryExtensions;
using ApiProject.Helpers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Query;


namespace ApiProject.Controllers
{
    public class LikesController : BaseController
    { 
        private readonly IUserRepository _userRepo;
        private readonly ILikesRepository _likesRepo;

        public LikesController(IUserRepository userRepository, ILikesRepository likesRepository)
        {
            _likesRepo = likesRepository;
            _userRepo = userRepository;
        }

        [HttpPost("{username}")]
        public async Task<ActionResult> AddLike(string username){
            var SourceUserId = User.GetUserId();
            var likeduser = await _userRepo.GetUserByNameAsync(username);
            var sourceuser = await _userRepo.GetUserByIdAsync(SourceUserId);

            if(likeduser == null) return NotFound();

            if(sourceuser.UserName == username) return BadRequest("you can like yourself , that wacky!");

            var userLike = await _likesRepo.GetUserLike(SourceUserId,likeduser.Id);

            if(userLike != null) return BadRequest ("you already like this user");

            userLike = new UserLike{
                SourceUserId = sourceuser.Id,
                TargetUserId = likeduser.Id         
            };

            sourceuser.LikedUsers.Add(userLike);
            
            if(await _userRepo.SaveAllAsync()) return Ok();
            
            return BadRequest("failed to save");

        }

        [HttpGet]
        public async Task<ActionResult<PageList<LikeDto>>> GetUserLikes([FromQuery] LikesParams likesParams)
        {
            likesParams.UserId = User.GetUserId();
            var users = await _likesRepo.GetUserLikes(likesParams);

            Response.AddPaginationHeader(new PaginationHeader(users.CurrentPage,users.PageSize,users.TotalCount,users.TotalPages));
            return Ok(users);
        }
    }
}

 
