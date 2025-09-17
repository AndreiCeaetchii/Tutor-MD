using System;

namespace Tutor.Application.Features.Tutors.Dto;

public class UpdateTutorProfileDto
{
    public string? Phone { get; set; }
    public string? Username { get; set; }
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public string? Bio { get; set; }
    public DateTime? Birthdate { get; set; }
    public string? City { get; set; }
    public string? Country { get; set; }
    public int? ExperienceYears { get; set; }
}