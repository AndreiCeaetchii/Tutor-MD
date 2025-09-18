using System;

namespace Tutor.Application.Features.Students.DTOs;

public class UpdateStudentProfileDto
{
    public string? Phone { get; set; }
    public string? Username { get; set; }
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public string? Bio { get; set; }
    public DateTime? Birthdate { get; set; }
    public string? City { get; set; }
    public string? Country { get; set; }
    public int? Grade { get; set; }
    public int? Class { get; set; }
}