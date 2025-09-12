using Ardalis.Result;
using MediatR;
using System.Threading;
using System.Threading.Tasks;
using Tutor.Application.Features.Students.DTOs;
using Tutor.Application.Interfaces;

namespace Tutor.Application.Features.Students.UpdateStudent;

public class UpdateStudentHandler : IRequestHandler<UpdateStudentCommand, Result<StudentDto>>
{
    private readonly IStudentService _studentService;

    public UpdateStudentHandler(IStudentService studentService)
    {
        _studentService = studentService;
    }

    public async Task<Result<StudentDto>> Handle(UpdateStudentCommand request, CancellationToken cancellationToken)
    {
        return await _studentService.UpdateStudentAsync(request.UserId, request.UpdateStudentProfileDto);
    }
}