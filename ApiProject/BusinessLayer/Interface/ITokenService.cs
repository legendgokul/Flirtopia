
using ApiProject.Entities;

namespace ApiProject.BusinessLayer.Interface;
public interface ITokenService
{
    string createToken(AppUser user);
}