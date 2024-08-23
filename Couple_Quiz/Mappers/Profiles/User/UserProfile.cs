using AutoMapper;
using Couple_Quiz.DTO.Request.Command.User;
using Couple_Quiz.DTO.Response.User;
using Couple_Quiz.Models;

namespace Couple_Quiz.Mappers.Profiles.Users
{
    public class UserProfile:Profile
    {
        public UserProfile()
        {

            // UserMap with Authresposne
            CreateMap<User, AuthUserResposne>()
              .ForMember(dest => dest.JwtToken, opt => opt.Ignore())
              .ForMember(dest => dest.RefreshToken, opt => opt.MapFrom(src => src.RefreshToken))
              .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.Name))
              .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email))
              .ReverseMap();
            // Map User m to CreateUserResponse
            CreateMap<User, CreateUserReposne>()
                .ForMember(dest => dest.UserId, opt => opt.MapFrom(src => src.GlobalId)) 
                .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email))
                .ForMember(dest => dest.Password, opt => opt.Ignore())
                .ForMember(dest => dest.ProfileImage, opt => opt.MapFrom(src => src.ProfileImage))
                .ForMember(dest => dest.Age, opt => opt.MapFrom(src => src.Age))
                .ForMember(dest => dest.Gender, opt => opt.MapFrom(src => src.Gender))
                .ReverseMap();
          
        }
    }
}
