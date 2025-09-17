using Ardalis.Result;
using MediatR;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Tutor.Application.Features.Students.DTOs;
using Tutor.Application.Interfaces;

namespace Tutor.Application.Features.Students.CreateStudent;

public class CreateStudentHandler : IRequestHandler<CreateStudentCommand, Result<StudentDto>>
{
    private readonly IStudentService _studentService;
    private readonly IUserService _userService;

    public CreateStudentHandler(IStudentService studentService,
        IUserService userService)
    {
        _studentService = studentService;
        _userService = userService;
    }
    public async Task<Result<StudentDto>> Handle(CreateStudentCommand request, CancellationToken cancellationToken)
    {
        var result = await _userService.UpdateProfileAsync(request.UserId, request.CreateStudentDto.CreateProfileDto);
        if (result.Errors.Any())
            return  Result<StudentDto>.Error(result.Errors.ToString());
        return await _studentService.CreateStudentAsync(request.CreateStudentDto, request.UserId);
    }
}