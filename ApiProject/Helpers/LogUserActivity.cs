using ApiProject.DataAccess.Interface;
using ApiProject.Extensions.LibraryExtensions;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.EntityFrameworkCore;

namespace ApiProject.Helpers
{
    // we are creating this middle ware only because we need to have latest last active date and time, to implement chat feature.
    // if latest active time is not required we can update last active during login.
    public class LogUserActivity :IAsyncActionFilter
    {
        public async Task OnActionExecutionAsync (ActionExecutingContext context, ActionExecutionDelegate next){

            var resultContext = await next();

            if(!resultContext.HttpContext.User.Identity.IsAuthenticated) return;

            var userId = resultContext.HttpContext.User.GetUserId();

            var repo = resultContext.HttpContext.RequestServices.GetRequiredService<IUserRepository>();

            var user = await repo.GetUserByIdAsync(userId);
            user.LastActive = DateTime.UtcNow;
            await repo.SaveAllAsync();
        }
    }
}