using Ardalis.Result;
using MediatR;
using Tutor.Application.Features.Students.DTOs;

namespace Tutor.Application.Features.Students.CreateStudent;

public  record CreateStudentCommand(int UserId, CreateStudentDto CreateStudentDto) : IRequest<Result<StudentDto>>;
