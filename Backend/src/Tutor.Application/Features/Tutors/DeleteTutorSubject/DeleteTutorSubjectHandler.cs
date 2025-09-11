using Ardalis.Result;
using MediatR;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Tutor.Application.Features.Tutors.Dto;
using Tutor.Application.Interfaces;

namespace Tutor.Application.Features.Tutors.DeleteTutorSubject;

public class DeleteTutorSubjectHandler : IRequestHandler<DeleteTutorSubjectCommand, Result<List<TutorSubjectDto>>>
{
    private readonly ITutorSubjectService  _tutorSubjectService;

    public DeleteTutorSubjectHandler(ITutorSubjectService tutorSubjectService)
    {
        _tutorSubjectService = tutorSubjectService;
    }
    public async Task<Result<List<TutorSubjectDto>>> Handle(DeleteTutorSubjectCommand request, CancellationToken cancellationToken)
    {
        return await _tutorSubjectService.DeleteTutorSubjectAsync(request.userId, request.subjectId);
    }
}