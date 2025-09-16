using AutoMapper;
using Tutor.Application.Features.Tutors.Dto;
using Tutor.Domain.Entities;

namespace Tutor.Application.Mappers;

public class AvailabilityMappingProfile : Profile
{
    public AvailabilityMappingProfile()
    {
        CreateMap<CreateAvailabilityDto, TutorAvailabilityRule>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.TutorUserId, opt => opt.Ignore());

        // Add this mapping for UPDATE operations
        CreateMap<AvailabilityDto, TutorAvailabilityRule>()
            .ForMember(dest => dest.Id, opt => opt.Ignore()) // Don't map ID from DTO to entity
            .ForMember(dest => dest.TutorUserId, opt => opt.Ignore()); // Don't map UserId from DTO

        CreateMap<TutorAvailabilityRule, AvailabilityDto>();
    }
}