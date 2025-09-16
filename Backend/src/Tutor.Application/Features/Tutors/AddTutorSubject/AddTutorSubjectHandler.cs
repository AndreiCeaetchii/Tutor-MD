using Ardalis.Result;
using MediatR;
using System.Threading;
using System.Threading.Tasks;
using Tutor.Application.Features.Tutors.Dto;
using Tutor.Application.Interfaces;

namespace Tutor.Application.Features.Tutors.AddTutorSubject;

public class AddTutorSubjectHandler : IRequestHandler<AddTutorSubjectCommand, Result<TutorSubjectDto>>
{
    private readonly ITutorSubjectService  _subjectService;

    public AddTutorSubjectHandler(ITutorSubjectService subjectService)
    {
        _subjectService = subjectService;
    }
    
    public async Task<Result<TutorSubjectDto>> Handle(AddTutorSubjectCommand request, CancellationToken cancellationToken)
    {
        return await _subjectService.AddTutorSubjectAsync(request.userId, request.tutorSubjectRequestDto);
    }
}