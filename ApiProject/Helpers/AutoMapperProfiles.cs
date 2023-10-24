using ApiProject.Data.CustomModels;
using ApiProject.Entities;
using AutoMapper;

namespace ApiProject.Helpers{
    public class AutoMapperProfiles : Profile{
        public AutoMapperProfiles()
        {
            CreateMap<AppUser,MemberDTO>()
            .ForMember(dest => dest.Age, opt => opt.MapFrom(src => CalculateAge(src.DateOfBirth)))
            .ForMember(dest => dest.photoUrl,
            opt => opt.MapFrom(src =>src.Photos.First(x=>x.IsMain).Url));

            CreateMap<Photo,PhotoDTO>();

            //Update mapper.
            CreateMap<MemberUpdateDto,AppUser>();

            //redister mapper
            CreateMap<RegisterDTO,AppUser>();
        }

        /// <summary>
        /// Calculate Age based on DataofBirth
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        public static int CalculateAge(DateTime date)
        {
                
            var today = DateTime.UtcNow; // Get today's DateOnly in UTC.F
            int age = today.Year - date.Year;

            if (date.Month > today.Month || (date.Month == today.Month && date.Day > today.Day))
            {
                age--; // The birthday hasn't occurred yet this year
            }

            return age;
        }
    }
}