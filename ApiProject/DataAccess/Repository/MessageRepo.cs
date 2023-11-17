using ApiProject.Data.AppContextFile;
using ApiProject.Data.CustomModels;
using ApiProject.DataAccess.Interface;
using ApiProject.Entities;
using ApiProject.Helpers;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;

namespace ApiProject.DataAccess.Repository
{
    public class MessageRepo : IMessageRepo
    {
        private readonly DatingAppContext _context;
        private readonly IMapper _mapper;
        public MessageRepo(DatingAppContext context , IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public void AddMessage(Message message)
        {
             _context.Messages.Add(message);
        }

        public void DeleteMessage(Message message)
        {
            _context.Remove(message);
        }

        public async Task<Message> GetMessage(int id)
        {
            return await _context.Messages.Include(x=>x.Sender).Include(x=>x.Recipient).FirstOrDefaultAsync(x =>x.Id == id);
        }

        public async Task<PageList<MessageDto>> GetMessageForUser(MessageParams MessageParams)
        {
            var query = _context.Messages
            .OrderByDescending(x=>x.MessageSent)
            .AsQueryable();

            query = MessageParams.Container switch
            {
                "Inbox" => query.Where(u=>u.RecipientUsername == MessageParams.Username && u.RecipientDeleted == false),
                "Outbox" => query.Where(u=>u.SenderUsername == MessageParams.Username && u.SenderDeleted == false),
                _ => query.Where(u=>u.RecipientUsername == MessageParams.Username 
                    && u.RecipientDeleted == false && u.DateRead == null)
            };

            var message = query.ProjectTo<MessageDto>(_mapper.ConfigurationProvider);

            return await PageList<MessageDto>.CreateAsync(message, MessageParams.PageNumber, MessageParams.PageSize);

        }

        public async Task<IEnumerable<MessageDto>> GetMessageThread(string currentUserName, string recipientName)
        {
            var messages = await _context.Messages
                    .Include(u => u.Sender).ThenInclude(p=>p.Photos)
                    .Include(u => u.Recipient).ThenInclude(p=>p.Photos)
                    .Where(
                        m => m.RecipientUsername == currentUserName  && 
                        m.RecipientDeleted == false &&
                        m.SenderUsername == recipientName ||
                        m.RecipientUsername == recipientName && 
                        m.SenderDeleted == false &&
                        m.SenderUsername == currentUserName
                    ).OrderBy(m => m.MessageSent).ToListAsync();

            var unreadMessages = messages.Where(m => m.DateRead == null && m.RecipientUsername == currentUserName).ToList();

            if(unreadMessages.Any())
            {
                foreach(var message in unreadMessages)
                {
                    message.DateRead = DateTime.UtcNow;

                }
                await _context.SaveChangesAsync();
            }

            return _mapper.Map<IEnumerable<MessageDto>>(messages);
        }

        public async Task<bool> SaveAllAsync()
        {
            
                var result = await _context.SaveChangesAsync();
                return result > 0;
            
        }
    }
}