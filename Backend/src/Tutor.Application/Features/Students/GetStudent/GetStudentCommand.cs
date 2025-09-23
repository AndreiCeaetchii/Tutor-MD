using Ardalis.Result;
using MediatR;
using Tutor.Application.Features.Students.DTOs;

namespace Tutor.Application.Features.Students.GetStudent;

public record GetStudentCommand(int UserId) : IRequest<Result<StudentDto>>;
