using Tutor.Application.Features.Tutors.Dto;
using Tutor.Application.Features.Users.Dtos;

namespace Tutor.Application.Features.Students.DTOs;

public class CreateStudentDto
{
    public int? Grade { get; set; }
    
    public int? Class { get; set; }
    
    public required CreateProfileDto CreateProfileDto { get; set; }
}