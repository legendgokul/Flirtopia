using ApiProject.Data.CustomModels;
using ApiProject.Entities;
using ApiProject.Helpers;

namespace ApiProject.DataAccess.Interface
{
    public interface IMessageRepo
    {
        void AddMessage(Message message);
        void DeleteMessage(Message message);
        Task<Message> GetMessage(int id);
        Task<PageList<MessageDto>> GetMessageForUser(MessageParams messageParams);
        Task<IEnumerable<MessageDto>> GetMessageThread(string currentUserName, string recipientName);
        Task<bool> SaveAllAsync();
    
    }
}