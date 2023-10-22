using ApiProject.Data.AppContextFile;
using ApiProject.Data.CustomModels;
using ApiProject.DataAccess.Interface;
using ApiProject.Entities;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace ApiProject.Controllers;

[Authorize]
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

    [HttpPut]
    [Route("UpdateUser")]
    public async Task<ActionResult> UpdateUser(MemberUpdateDto memberUpdateDto)
    {
        var currentUser = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        var user = await _userRepo.GetUserByNameAsync(currentUser);

        if(user == null){
            return NotFound();
        }

        mapper.Map(memberUpdateDto,user);

        if(await _userRepo.SaveAllAsync()) return NoContent();

        return BadRequest("failed to update user");

    }
}

