using ApiProject.DataAccess;
using ApiProject.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ApiProject.Controllers;

[ApiController]
[Route("api/[controller]")] 
public class UserController : ControllerBase
{
    private DatingAppContext _context;
    public UserController(DatingAppContext context)
    {
        this._context = context;
    }

    [HttpGet]
    [Route("GetUserList")]
    public async Task<ActionResult<List<AppUser>>> getAppUser()
    {
        var User = await _context.appUser.ToListAsync();

        return Ok(User);
    }
}

