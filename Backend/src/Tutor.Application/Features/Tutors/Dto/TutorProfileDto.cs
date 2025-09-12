using System.Collections.Generic;
using Tutor.Application.Features.Photos.DTOs;
using Tutor.Application.Features.Users.Dtos;
using Tutor.Domain.Entities;

namespace Tutor.Application.Features.Tutors.Dto;

public class TutorProfileDto
{
    public int UserId { get; set; }
    public VerificationStatus VerificationStatus { get; set; }
    public int? ExperienceYears { get; set; }
    public int role {get; set;} = 2;
    public List<TutorSubjectDto> TutorSubjects { get; set; } = new List<TutorSubjectDto>();
    public required CreateProfileDto UserProfile { get; set; }
    
    public PhotoDto? Photo {get; set;}
}

