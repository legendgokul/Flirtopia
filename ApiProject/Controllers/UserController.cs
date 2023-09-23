using ApiProject.DataAccess;
using ApiProject.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ApiProject.Controllers;

public class UserController : BaseController
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

    [HttpGet]
    [Route("GetUserID/{id}")]
    public async Task<ActionResult<AppUser>> GetUserID(int id)
    {
        var User = await _context.appUser.Where(x=>x.Id ==  id).FirstOrDefaultAsync();

        return Ok(User);
    }
}

