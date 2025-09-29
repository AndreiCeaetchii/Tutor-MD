using System.ComponentModel.DataAnnotations;
using Tutor.Application.Features.Tutors.Dto;
using Tutor.Application.Features.Users.Dtos;

namespace Tutor.Application.Features.Students.DTOs;

public class CreateStudentDto
{
    [Range(1, 12, ErrorMessage = "Grade must be between 1 and 12")]
    public int? Grade { get; set; }

    [Range(1, 20, ErrorMessage = "Class must be between 1 and 20")]
    public int? Class { get; set; }
    
    public required CreateProfileDto CreateProfileDto { get; set; }
}