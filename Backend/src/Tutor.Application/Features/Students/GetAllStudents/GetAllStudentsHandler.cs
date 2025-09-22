using Ardalis.Result;
using MediatR;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Tutor.Application.Features.Students.DTOs;
using Tutor.Application.Interfaces;

namespace Tutor.Application.Features.Students.GetAllStudents;

public class GetAllStudentsHandler : IRequestHandler<GetAllStudentsCommand, Result<List<StudentDto>>>
{
    private readonly IStudentService  _studentService;

    public GetAllStudentsHandler(IStudentService studentService)
    {
        _studentService = studentService;
    }
    public async Task<Result<List<StudentDto>>> Handle(GetAllStudentsCommand request, CancellationToken cancellationToken)
    {
        return await _studentService.GetAllStudentsAsync();
    }
}