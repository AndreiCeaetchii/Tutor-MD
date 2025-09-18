using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Tutor.Application.Features.Users.Dtos;
using Tutor.Domain.Entities;

namespace Tutor.Application.Features.Tutors.Dto;

public class CreateTutorProfileDto
{
    public VerificationStatus VerificationStatus { get; set; } = VerificationStatus.Pending;

    [Range(0, 50, ErrorMessage = "Experience years must be between 0 and 50")]
    public int? ExperienceYears { get; set; }

    [Required(ErrorMessage = "At least one subject is required")]
    [MinLength(1, ErrorMessage = "At least one subject is required")]
    public List<TutorSubjectRequestDto> Subjects { get; set; } = new List<TutorSubjectRequestDto>();
    
    public required CreateProfileDto CreateProfileDto {get; set;}
}