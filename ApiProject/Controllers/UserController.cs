using ApiProject.BusinessLayer.Interface;
using ApiProject.Data.CustomModels;
using ApiProject.DataAccess.Interface;
using ApiProject.Entities;
using ApiProject.Extensions;
using ApiProject.Extensions.LibraryExtensions;
using ApiProject.Helpers;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;


namespace ApiProject.Controllers;

[Authorize]
public class UserController : BaseController
{
    private readonly IUserRepository _userRepo;
    private readonly IMapper _mapper;
    private readonly IPhotoService _photoService;


    public UserController(IUserRepository userRepository, 
    IMapper mapper,IPhotoService photoService)
    {
        this._userRepo = userRepository;
        this._mapper = mapper;
        this._photoService = photoService;
    }

    [HttpGet]
    [Route("GetUserList")]
    public async Task<ActionResult<PageList<MemberDTO>>> getAppUser([FromQuery] UserParams userParams)
    {
      /*  var currentuser = await _userRepo.GetUserByNameAsync(User.GetUserName());
        userParams.CurrentUsername = currentuser.UserName;

        if(string.IsNullOrEmpty(userParams.Gender)){
            userParams.Gender = (currentuser.Gender == "male")?"female":"male";
        }
        */
        var Users = await _userRepo.GetMembersAsync(userParams);

        Response.AddPaginationHeader(new PaginationHeader(
            Users.CurrentPage,
            Users.PageSize,
            Users.TotalCount,
            Users.TotalPages
            ));    

        return Ok(Users);
    }

    [HttpGet]
    [Route("GetUserByName/{userName}")]
    public async Task<ActionResult<MemberDTO>> GetUser(string userName)
    {
        var User = await _userRepo.GetMemberByNameAsync(userName);
        return Ok(User);
    }

    [HttpPut]
    [Route("UpdateUser")]
    public async Task<ActionResult> UpdateUser(MemberUpdateDto memberUpdateDto)
    {
        //User.GetUserName() custom library extension 
        var user = await _userRepo.GetUserByNameAsync(User.GetUserName());

        if(user == null){
            return NotFound();
        }

        _mapper.Map(memberUpdateDto,user);

        if(await _userRepo.SaveAllAsync()) return NoContent();

        return BadRequest("failed to update user");

    }

    [HttpPost]
    [Route("add-photo")]
    public async Task<ActionResult<PhotoDTO>> AddPhoto(IFormFile file)
    {
        // fetch the user from Database using username obtained from userIdentity from jwt
        var currentuser = await _userRepo.GetUserByNameAsync(User.GetUserName());
        
        if(User == null) return NotFound();
        
        // update photo and retrive the imageUploadREsponse from cloudinary.
        var uploadResponse = await _photoService.AddPhotoAsync(file);
        
        //Check the response to see if we got Error.
        if(uploadResponse.Error != null) return BadRequest(uploadResponse.Error.Message);
        
        //create a photo variable to insert
        var photo = new Photo(){
            Url = uploadResponse.Url.AbsoluteUri,
            PublicId = uploadResponse.PublicId
        };

        //check if this is first image of current user to set imain property.
        if(currentuser.Photos.Count()==0) photo.IsMain = true;

        currentuser.Photos.Add(photo);

        //save the changes to DB.
        if(await _userRepo.SaveAllAsync()) {

            return CreatedAtAction(nameof(GetUser),
                new {userName = currentuser.UserName},
                _mapper.Map<PhotoDTO>(photo));
                
            }
       
        //if the save is not success then return bad request
        return BadRequest("problem adding photo to current user.");
    }


    [HttpPut]
    [Route("set-main-photo/{photoId}")]
    public async Task<ActionResult> SetMainPhoto(int photoId)
    {
         var currentuser = await _userRepo.GetUserByNameAsync(User.GetUserName());
        
        if(currentuser == null){
            return NotFound();
        }

        var photo = currentuser.Photos.FirstOrDefault(x=>x.id == photoId);

        if(photo == null){
            return NotFound();
        }

        if(photo.IsMain) return BadRequest("this is already your main photo");

        var currentMain = currentuser.Photos.FirstOrDefault(x=>x.IsMain);

        if(currentMain != null ) currentMain.IsMain = false;
        
        photo.IsMain = true;
         if(await _userRepo.SaveAllAsync()) {
            return NoContent();
         }
         return BadRequest("Problem setting the main photo");
    }

    [HttpDelete]
    [Route("delete-photo/{photoId}")]
    public async Task<ActionResult> DeletePhoto(int photoId){
        
        var currentuser = await _userRepo.GetUserByNameAsync(User.GetUserName());
        
        if(currentuser == null){
            return NotFound();
        }

        var photo = currentuser.Photos.FirstOrDefault(x=>x.id == photoId);

        if(photo == null){
            return NotFound();
        }

        if(photo.IsMain) return BadRequest("you cant delete your main pic");

        if(photo.PublicId != null){
            var result = await _photoService.DeletePhotoAsync(photo.PublicId);
            if(result.Error != null ) BadRequest(result.Error.Message);
        }

        currentuser.Photos.Remove(photo);
        if(await _userRepo.SaveAllAsync()){
            return Ok();
        }else{
            return BadRequest("Problem deleting photos");
        }
    }
}

