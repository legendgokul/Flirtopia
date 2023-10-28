using ApiProject.Data.CustomModels;
using ApiProject.Entities;
using ApiProject.Helpers;

namespace ApiProject.DataAccess.Interface{
    public interface IUserRepository{
        Task<bool> SaveAllAsync();
        Task<IEnumerable<AppUser>> GetUsersAsync();
        Task<AppUser> GetUserByIdAsync(int id);
        Task<AppUser> GetUserByNameAsync(string name);
        Task<MemberDTO> GetMemberByNameAsync(string name);
        Task<PageList<MemberDTO>> GetMembersAsync(UserParams userParams);
    }
}