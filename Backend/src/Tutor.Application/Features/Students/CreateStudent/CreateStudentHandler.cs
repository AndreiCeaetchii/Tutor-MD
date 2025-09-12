using Ardalis.Result;
using MediatR;
using System.Threading;
using System.Threading.Tasks;
using Tutor.Application.Features.Students.DTOs;
using Tutor.Application.Interfaces;

namespace Tutor.Application.Features.Students.CreateStudent;

public class CreateStudentHandler : IRequestHandler<CreateStudentCommand, Result<StudentDto>>
{
    private readonly IStudentService _studentService;

    public CreateStudentHandler(IStudentService studentService)
    {
        _studentService = studentService;
    }
    public async Task<Result<StudentDto>> Handle(CreateStudentCommand request, CancellationToken cancellationToken)
    {
        return await _studentService.CreateStudentAsync(request.CreateStudentDto, request.UserId);
    }
}