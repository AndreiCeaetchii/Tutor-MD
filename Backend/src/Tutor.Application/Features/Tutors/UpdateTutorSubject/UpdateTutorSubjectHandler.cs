using Ardalis.Result;
using MediatR;
using System.Threading;
using System.Threading.Tasks;
using Tutor.Application.Features.Tutors.Dto;
using Tutor.Application.Interfaces;

namespace Tutor.Application.Features.Tutors.UpdateTutorSubject;

public class UpdateTutorSubjectHandler : IRequestHandler<UpdateTutorSubjectCommand, Result<TutorSubjectDto>>
{
    private readonly ITutorSubjectService _tutorSubjectService;

    public UpdateTutorSubjectHandler(ITutorSubjectService tutorSubjectService)
    {
        _tutorSubjectService = tutorSubjectService;
    }

    public async Task<Result<TutorSubjectDto>> Handle(UpdateTutorSubjectCommand request, CancellationToken cancellationToken)
    {
        return await _tutorSubjectService.UpdateTutorSubjectAsync(request.userId, request.tutorSubjectDto);
    }
}