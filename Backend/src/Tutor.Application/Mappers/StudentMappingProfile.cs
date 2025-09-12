using AutoMapper;
using Tutor.Application.Features.Photos.DTOs;
using Tutor.Application.Features.Students.DTOs;
using Tutor.Application.Features.Users.Dtos;
using Tutor.Domain.Entities;

namespace Tutor.Application.Mappers;

public class StudentMappingProfile : Profile
{
    public StudentMappingProfile()
    {
        // TutorProfile -> TutorProfileDto
        CreateMap<Student, StudentDto>()
            .ForMember(dest => dest.UserProfile, 
                opt => opt.MapFrom(src => src.User))
            .ForMember(dest => dest.Photo,
                opt => opt.MapFrom(src => src.User.Photo));

      

        CreateMap<User, CreateProfileDto>();
        
        CreateMap<Photo, PhotoDto>();
    }
    
    
}