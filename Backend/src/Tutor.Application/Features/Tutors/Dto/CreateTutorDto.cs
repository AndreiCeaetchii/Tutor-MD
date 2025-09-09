using Tutor.Domain.Entities;

namespace Tutor.Application.Features.Tutors.Dto;

public class CreateTutorDto
{
    public VerificationStatus VerificationStatus { get; set; }
    
    public int? ExperienceYears { get; set; }
    
    
}