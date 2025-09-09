using System.Collections.Generic;
using Tutor.Domain.Entities;

namespace Tutor.Application.Features.Tutors.Dto;

public class TutorProfileDto
{
    public int UserId { get; set; }
    public VerificationStatus VerificationStatus { get; set; }
    public int? ExperienceYears { get; set; }
    public List<TutorSubjectDto> TutorSubjects { get; set; } = new List<TutorSubjectDto>();
}

