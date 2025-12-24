using AutoMapper;
using SchoolManagementSystem.Application.DTOs.Auth;
using SchoolManagementSystem.Application.Responses.Auth;
using SchoolManagementSystem.Application.ViewModels;
using SchoolManagementSystem.Domain.Enums;
using SchoolManagementSystem.Domain.Models;

namespace SchoolManagementSystem.Application.Helpers
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<Department, DepartmentViewModel>();
            CreateMap<Course, CourseViewModel>();

            CreateMap<RegisterRequestDTO, User>()
            .ForMember(dest => dest.Role,
                opt => opt.MapFrom(src => Enum.Parse<Role>(src.Role, true)))
            .ForMember(dest => dest.PasswordHash, opt => opt.Ignore())
            .ForMember(dest => dest.PasswordSalt, opt => opt.Ignore());

            CreateMap<(User user, string accessToken, string refreshToken, string expires), AuthResponse>()
            .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.user.Email))
            .ForMember(dest => dest.Role, opt => opt.MapFrom(src => src.user.Role.ToString()))
            .ForMember(dest => dest.AccessToken, opt => opt.MapFrom(src => src.accessToken))
            .ForMember(dest => dest.RefreshToken, opt => opt.MapFrom(src => src.refreshToken))
             .ForMember(dest => dest.Expires, opt => opt.MapFrom(src => src.expires));
        }
    }
}
