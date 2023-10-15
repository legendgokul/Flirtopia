using ApiProject.Data.AppContextFile;
using ApiProject.Data.CustomModels;
using ApiProject.DataAccess.Interface;
using ApiProject.Entities;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ApiProject.Controllers;

//[Authorize]
public class UserController : BaseController
{
    private readonly IUserRepository _userRepo;
    private readonly IMapper mapper;
    public UserController(IUserRepository userRepository, IMapper mapper)
    {
        this._userRepo = userRepository;
        this.mapper = mapper;
    }

    [HttpGet]
    [Route("GetUserList")]
    public async Task<ActionResult<List<MemberDTO>>> getAppUser()
    {
        var Users = await _userRepo.GetMembersAsync();
        return Ok(Users);
    }

    [HttpGet]
    [Route("GetUserByName/{userName}")]
    public async Task<ActionResult<MemberDTO>> GetUserID(string userName)
    {
        var User = await _userRepo.GetMemberByNameAsync(userName);
        return Ok(User);
    }
}

