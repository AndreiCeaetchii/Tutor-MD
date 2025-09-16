using System;

namespace Tutor.Application.Features.Users.Dtos;

public class CreateProfileDto
{
    public string? Phone { get; set; }
    public string? Username { get; set; }
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public string? Bio { get; set; }
    public string? City { get; set; }
    public string? Country { get; set; }
    public DateTime? Birthdate { get; set; }
}