using System.Security.Cryptography;
using System.Text;
using ApiProject.DataAccess;
using ApiProject.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ApiProject.Controllers;

public class AccountController: BaseController{
     
     private readonly DatingAppContext _context;
     public AccountController(DatingAppContext context){
        this._context = context;
     }


     [HttpPost]
     [Route("register")]
     public async Task<ActionResult<AppUser>> Register(RegisterDTO registerInfo)
     {      

        if(await userExists(registerInfo.userName.ToLower())){ return BadRequest("UserName Taken !!!");} 
        using var hmac = new HMACSHA512();  // create a instance which will generate 1 key which is used for Salting hash
        
        var user = new AppUser{
            UserName = registerInfo.userName,
            Email = registerInfo.userName +"@gmail.com",
            PasswordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(registerInfo.password)), //cpnvert password string -> byte using a random generated key
            PasswordSalt = hmac.Key // each instance of HMACSHA512 will have a key auto generated.
        };
        _context.appUser.Add(user);
        await _context.SaveChangesAsync();

        return user;
     }

      [HttpPost]
      [Route("Login")]
      public async Task<ActionResult<AppUser>> Login (LoginDTO loginDTO)
      {
         //SingleOrDefault will throw exception if there are 2 or more items identified for the filter which we applied
         var user = await _context.appUser.SingleOrDefaultAsync(x=>x.UserName.ToLower() == loginDTO.username.ToLower());
         if(user == null){
            return Unauthorized("UserName Does not Exist!");  // returning unauthorised because provided name is not saved in Db. 
         }
         
         using var hmac = new HMACSHA512(user.PasswordSalt); // we are using same salt inorder to produce same hash
         var paswordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(loginDTO.password));
         
         //compare both byte string to check if they both are same
         for(int i=0;i<paswordHash.Length;i++){
            if(paswordHash[i] != user.PasswordHash[i]) return Unauthorized("Password does not Match!");
         }

         return user;
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