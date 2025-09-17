namespace Tutor.Application.Features.Students.DTOs;

public class AuthStudentResponseDto
{
    public StudentDto Student { get; set; }
    public string Token { get; set; }
    
    public AuthStudentResponseDto(StudentDto student, string token)
    {
        Student = student;
        Token = token;
    }
}