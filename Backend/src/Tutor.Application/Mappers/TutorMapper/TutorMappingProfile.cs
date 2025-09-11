using Tutor.Application.Features.Tutors.Dto;
using Tutor.Domain.Entities;
using AutoMapper;
using Tutor.Application.Features.Photos.DTOs;
using Tutor.Application.Features.Users.Dtos;


namespace Tutor.Application.Mappers.TutorMapper;

public class TutorMappingProfile : Profile
{
    public TutorMappingProfile()
    {
        // TutorProfile -> TutorProfileDto
        CreateMap<TutorProfile, TutorProfileDto>()
            .ForMember(dest => dest.UserProfile, 
                opt => opt.MapFrom(src => src.User))
            .ForMember(dest => dest.Photo,
                opt => opt.MapFrom(src => src.User.Photo));

        // TutorSubject -> TutorSubjectDto
        CreateMap<TutorSubject, TutorSubjectDto>()
            .ForMember(dest => dest.SubjectName, 
                opt => opt.MapFrom(src => src.Subject.Name))
            .ForMember(dest => dest.SubjectSlug, 
                opt => opt.MapFrom(src => src.Subject.Slug));

        CreateMap<User, CreateProfileDto>();
        
        
        CreateMap<TutorSubject, TutorSubjectDto>();
        
        CreateMap<UpdateTutorProfileDto, CreateProfileDto>();
        CreateMap<Photo, PhotoDto>();
    }
    
    
}