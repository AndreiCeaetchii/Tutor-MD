using Ardalis.Result;
using MediatR;
using System.Threading;
using System.Threading.Tasks;
using Tutor.Application.Features.Tutors.Dto;
using Tutor.Application.Interfaces;

namespace Tutor.Application.Features.Tutors.UpdateTutorProfile;

public class UpdateTutorProfileHandler : IRequestHandler<UpdateTutorProfileCommand, Result<TutorProfileDto>>
{
    private readonly ITutorService _tutorService;

    public UpdateTutorProfileHandler(ITutorService tutorService)
    {
        _tutorService = tutorService;
    }

    public async Task<Result<TutorProfileDto>> Handle(UpdateTutorProfileCommand request,
        CancellationToken cancellationToken)
    {
        return await _tutorService.UpdateTutorAsync(request.userId, request.updateTutorProfileDto);
    }
}