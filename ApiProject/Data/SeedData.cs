using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using Newtonsoft.Json;
using ApiProject.Data.AppContextFile;
using ApiProject.Entities;
using Microsoft.EntityFrameworkCore;
using System.Text.Json.Serialization;
using System.Net.Http.Json;

namespace ApiProject.Data{
    public class SeedData{
        public static async Task SeedUser(DatingAppContext _context)
        {
            if (!await _context.appUser.AnyAsync())  // checking if the appuser table has any user or not.
            {
                
                    //read the seed json file 
                    var data = await File.ReadAllTextAsync("Data/UserSeedData.json");

                    //deserializee the string into custom model  
                    var ParsedData = JsonConvert.DeserializeObject<List<AppUser>>(data);
                    //insert it into respective tables

                    foreach (var user in ParsedData)
                    {

                        using var Hmac = new HMACSHA512();

                        user.UserName = user.UserName.ToLower();
                        user.Email = user.UserName + "@gmail.com";
                        user.PasswordHash = Hmac.ComputeHash(Encoding.UTF8.GetBytes("PassW0rd"));
                        user.PasswordSalt = Hmac.Key;
                        _context.appUser.Add(user);

                    }
                    await _context.SaveChangesAsync();

                
            }
        }
    }
}