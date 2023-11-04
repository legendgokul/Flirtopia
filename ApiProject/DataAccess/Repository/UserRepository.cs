using ApiProject.Data.AppContextFile;
using ApiProject.Data.CustomModels;
using ApiProject.DataAccess.Interface;
using ApiProject.Entities;
using ApiProject.Helpers;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;

namespace ApiProject.DataAccess.Repository{
    public class UserRepository : IUserRepository
    {
        private readonly DatingAppContext _context;
        private readonly IMapper _mapper;
        public UserRepository(DatingAppContext context, IMapper mapper){
            _context = context;
            _mapper = mapper;
        }

        public async Task<MemberDTO> GetMemberByNameAsync(string name)
        {
            return await _context.appUser
            .Where(x=>x.UserName == name.ToLower())
            .ProjectTo<MemberDTO>(_mapper.ConfigurationProvider)
            .FirstOrDefaultAsync();
        }

        public async Task<PageList<MemberDTO>> GetMembersAsync(UserParams userParams)
        {
            var Query =  _context.appUser.AsQueryable();

            // eager loading is missing here because ProjectTo takes care of all relationship 
            Query = Query.Where(u => u.UserName != userParams.CurrentUsername);
            Query = Query.Where(u => u.Gender == userParams.Gender);

            //Age filter 
            var minDob =  DateTime.UtcNow.AddYears(-userParams.MaxAge);
            var MaxDob =  DateTime.UtcNow.AddYears(-userParams.MinAge);

            Query = Query.Where(u => u.DateOfBirth >= minDob && u.DateOfBirth <= MaxDob);
            Query = Query.Where(u => u.Gender == userParams.Gender);
            
            Query = userParams.orderBy switch {
                "createdon" => Query.OrderByDescending(u => u.CreatedOn),
                _ => Query.OrderByDescending(u => u.LastActive)
            };

            
            return await PageList<MemberDTO>.CreateAsync(
                Query.AsNoTracking()
                .ProjectTo<MemberDTO>(_mapper.ConfigurationProvider), 
                userParams.PageNumber, 
                userParams.PageSize);
        }

        public async Task<AppUser> GetUserByIdAsync(int id)
        {
            return await _context.appUser.FindAsync(id);
        }

        public async Task<AppUser> GetUserByNameAsync(string name)
        {
            return await _context.appUser
            .Include(p =>p.Photos) // we are eager loading to join photos table to return respective data.
            .FirstOrDefaultAsync(x=>x.UserName == name.ToLower());
        }

        public async Task<IEnumerable<AppUser>> GetUsersAsync()
        {
            return await _context.appUser
            .Include(p =>p.Photos)
            .ToListAsync();
        }

        public async Task<bool> SaveAllAsync()
        {
            return await _context.SaveChangesAsync() > 0; // save changes returns int value of how many count are saved , and negative number in case of any exception.
        }
    }
}