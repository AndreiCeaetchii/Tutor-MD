using Tutor.Application.Features.Photos.DTOs;
using Tutor.Application.Features.Users.Dtos;

namespace Tutor.Application.Features.Students.DTOs;

public class StudentDto
{
    public int? Grade { get; set; }
    
    public int? Class { get; set; }
    public int role { get; set; } = 1;
    
    public required CreateProfileDto UserProfile { get; set; }
    
    public PhotoDto? Photo {get; set;}
}
