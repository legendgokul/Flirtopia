using System.Security.Claims;

namespace ApiProject.Extensions.LibraryExtensions
{
    public static class ClaimsPrincipalExtension{

        public static string GetUserName(this ClaimsPrincipal claims){
            return claims.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        }
    }
}