using System.Security.Cryptography;
using System.Text;
using ApiProject.BusinessLayer.Interface;
using ApiProject.Data.CustomModels;
using ApiProject.Data.AppContextFile;
using ApiProject.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using AutoMapper;

namespace ApiProject.Controllers;

public class AccountController: BaseController{
     
     private readonly DatingAppContext _context;
     private readonly ITokenService _tokenService;
     public readonly IMapper _mapper;
     
     public AccountController(DatingAppContext context, ITokenService tokenService, IMapper mapper){
        this._context = context;
        this._tokenService = tokenService;
        this._mapper = mapper;
     }


     [HttpPost]
     [Route("register")]
     public async Task<ActionResult<UserDTO>> Register(RegisterDTO registerInfo)
     {      

        if(await userExists(registerInfo.username.ToLower())){ return BadRequest("UserName Taken !!!");} 
        
        var user = _mapper.Map<AppUser>(registerInfo);
        
        using var hmac = new HMACSHA512();  // create a instance which will generate 1 key which is used for Salting hash
        
         user.UserName = registerInfo.username;
         user.Email = registerInfo.username +"@gmail.com";
         user.PasswordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(registerInfo.password)); //cpnvert password string -> byte using a random generated key
         user.PasswordSalt = hmac.Key; // each instance of HMACSHA512 will have a key auto generated.
        
        _context.appUser.Add(user);
        await _context.SaveChangesAsync();

        return new UserDTO{
         userName = user.UserName,
         token = _tokenService.createToken(user),
         KnownAs = user.knownAs

        };
     }

      [HttpPost]
      [Route("Login")]
      public async Task<ActionResult<UserDTO>> Login (LoginDTO loginDTO)
      {
         //SingleOrDefault will throw exception if there are 2 or more items identified for the filter which we applied
         var user = await _context.appUser
            .Include(p =>p.Photos) // we are eager loading to join photos table to return respective data.
            .FirstOrDefaultAsync(x=>x.UserName == loginDTO.username.ToLower());
            
         if(user == null){
            return Unauthorized("UserName Does not Exist!");  // returning unauthorised because provided name is not saved in Db. 
         }
         
         using var hmac = new HMACSHA512(user.PasswordSalt); // we are using same salt inorder to produce same hash
         var paswordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(loginDTO.password));
         
         //compare both byte string to check if they both are same
         for(int i=0;i<paswordHash.Length;i++){
            if(paswordHash[i] != user.PasswordHash[i]) return Unauthorized("Password does not Match!");
         }

       return new UserDTO{
         userName = user.UserName,
         token = _tokenService.createToken(user),
         PhotoUrl = user.Photos.FirstOrDefault(x=>x.IsMain ==true)?.Url,
         KnownAs = user.knownAs
        };
      }




      #region private Method
      /// <summary>
      ///  Api which indicates if UserName already registered or not.
      /// </summary>
      /// <param name="userName"></param>
      /// <returns></returns>
     private async Task<bool> userExists(String userName)
     {
         return await _context.appUser.AnyAsync(x=>x.UserName.ToLower() == userName);
     }
     #endregion
}