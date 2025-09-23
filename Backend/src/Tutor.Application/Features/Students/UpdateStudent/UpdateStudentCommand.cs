using Ardalis.Result;
using MediatR;
using Tutor.Application.Features.Students.DTOs;

namespace Tutor.Application.Features.Students.UpdateStudent;

public record UpdateStudentCommand(int UserId, UpdateStudentProfileDto UpdateStudentProfileDto) : IRequest<Result<StudentDto>>;
