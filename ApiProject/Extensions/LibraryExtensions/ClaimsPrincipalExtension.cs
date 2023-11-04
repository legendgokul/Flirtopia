using System.Security.Claims;

namespace ApiProject.Extensions.LibraryExtensions
{
    public static class ClaimsPrincipalExtension{

        public static string GetUserName(this ClaimsPrincipal claims){
            return claims.FindFirst(ClaimTypes.Name)?.Value;
        }

        public static int GetUserId(this ClaimsPrincipal claims){
            return int.Parse(claims.FindFirst(ClaimTypes.NameIdentifier)?.Value);
        }
    }
}