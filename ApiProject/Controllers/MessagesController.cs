using System.Net.Mail;
using ApiProject.Data.CustomModels;
using ApiProject.DataAccess.Interface;
using ApiProject.Entities;
using ApiProject.Extensions;
using ApiProject.Extensions.LibraryExtensions;
using ApiProject.Helpers;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ApiProject.Controllers
{
    [Authorize]
    public class MessagesController : BaseController
    {
        private readonly IUserRepository _userRepo;
        private readonly IMessageRepo _messageRepo;
        private readonly IMapper _mapper;
        public MessagesController(IUserRepository userRepository , IMessageRepo messageRepo , IMapper mapper)
        {
            _userRepo =userRepository;
            _messageRepo = messageRepo;
            _mapper = mapper;

        }

        [HttpPost]
        public async Task<ActionResult<MessageDto>> CreateMessage(CreateMessageDto createMessageDto)
        {
            var username = User.GetUserName();
            if(username == createMessageDto.RecipientUsername.ToLower()) return BadRequest("cant send message to self");

            var sender = await _userRepo.GetUserByNameAsync(username);
            var recipient = await _userRepo.GetUserByNameAsync(createMessageDto.RecipientUsername);

            if(recipient == null) return NotFound();

            var message = new Message
            {
                Sender = sender,
                Recipient = recipient,
                SenderUsername = sender.UserName,
                RecipientUsername = recipient.UserName,
                Content = createMessageDto.Content
            };

            _messageRepo.AddMessage(message);

            if(await _messageRepo.SaveAllAsync()) return Ok(_mapper.Map<MessageDto>(message));

            return BadRequest("Failed to send message");

        }


        [HttpGet]
        public async Task<ActionResult<PageList<MessageDto>>> GetMessageForUser( [FromQuery] MessageParams messageParams)
        {
            messageParams.Username = User.GetUserName();

            var message = await _messageRepo.GetMessageForUser(messageParams);

            Response.AddPaginationHeader(new PaginationHeader(message.CurrentPage,message.PageSize,message.TotalCount, message.TotalPages));

            return message;
        }

        [HttpGet("thread/{username}")]
        public async Task<ActionResult<IEnumerable<MessageDto>>> GetMessageThread(string username)
        {
            var currentUserName = User.GetUserName();
            return Ok(await _messageRepo.GetMessageThread(currentUserName, username));
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteMessage(int id)
        {
            var username = User.GetUserName();

            var message = await _messageRepo.GetMessage(id);

            if(message.SenderUsername != username && message.RecipientUsername != username)   
                return Unauthorized();

            if(message.SenderUsername == username) message.SenderDeleted = true;

            if(message.RecipientUsername == username) message.RecipientDeleted = true;

            if(message.SenderDeleted && message.RecipientDeleted)
            {
                _messageRepo.DeleteMessage(message);
            }

            if(await _messageRepo.SaveAllAsync()) return Ok();

            return BadRequest("Problem");           
        }


    }
}