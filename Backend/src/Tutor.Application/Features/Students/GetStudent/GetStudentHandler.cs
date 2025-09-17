using Ardalis.Result;
using MediatR;
using System.Threading;
using System.Threading.Tasks;
using Tutor.Application.Features.Students.DTOs;
using Tutor.Application.Interfaces;

namespace Tutor.Application.Features.Students.GetStudent;

public class GetStudentHandler :  IRequestHandler<GetStudentCommand, Result<StudentDto>>
{private readonly IStudentService  _studentService;

    public GetStudentHandler(IStudentService studentService)
    {
        _studentService = studentService;
    }
    
    public async Task<Result<StudentDto>> Handle(GetStudentCommand request, CancellationToken cancellationToken)
    {
        return await _studentService.GetStudentAsync(request.UserId);
    }
}