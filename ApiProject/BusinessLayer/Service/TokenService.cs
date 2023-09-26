using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using ApiProject.BusinessLayer.Interface;
using ApiProject.Entities;
using Microsoft.IdentityModel.Tokens;

namespace ApiProject.BusinessLayer.Service;
public class TokenService : ITokenService
{
    private readonly SymmetricSecurityKey _key;
    public TokenService(IConfiguration _config)
    {
        _key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["TokenKey"]));
    }


    public string createToken(AppUser user)
    {
        //claims can have userid, their role , this can hold other custom details too
        var claims = new List<Claim>{
            new Claim(JwtRegisteredClaimNames.NameId,user.UserName)
        };

        // create a signcred by passing key and signature algo
        var signcreds = new SigningCredentials(_key,SecurityAlgorithms.HmacSha512Signature);

        // create a desc object which we pass as parameter to create a token.
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(claims),
            Expires = DateTime.Now.AddDays(7),
            SigningCredentials = signcreds
        };
        
        var tokenHandler = new JwtSecurityTokenHandler();
        
        var token = tokenHandler.CreateToken(tokenDescriptor);

        //send the token to Ui
        return tokenHandler.WriteToken(token);
    }
    
}