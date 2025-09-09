using Tutor.Application.Features.Tutors.Dto;
using Tutor.Domain.Entities;
using AutoMapper;


namespace Tutor.Application.Mappers.TutorMapper;

public class TutorMappingProfile : Profile
{
    public TutorMappingProfile()
    {
        // TutorProfile -> TutorProfileDto
        CreateMap<TutorProfile, TutorProfileDto>();

        // TutorSubject -> TutorSubjectDto
        CreateMap<TutorSubject, TutorSubjectDto>()
            .ForMember(dest => dest.SubjectName, 
                opt => opt.MapFrom(src => src.Subject.Name))
            .ForMember(dest => dest.SubjectSlug, 
                opt => opt.MapFrom(src => src.Subject.Slug));
    }
}