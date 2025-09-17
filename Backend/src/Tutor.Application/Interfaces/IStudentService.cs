using Ardalis.Result;
using System.Threading.Tasks;
using Tutor.Application.Features.Students.DTOs;

namespace Tutor.Application.Interfaces;

public interface IStudentService
{
    Task<Result<StudentDto>> CreateStudentAsync(CreateStudentDto createStudentDto, int userId);
    Task<Result<StudentDto>> GetStudentAsync(int userId);

    Task<Result<StudentDto>> UpdateStudentAsync(int userId, UpdateStudentProfileDto updateStudentProfileDto);
}