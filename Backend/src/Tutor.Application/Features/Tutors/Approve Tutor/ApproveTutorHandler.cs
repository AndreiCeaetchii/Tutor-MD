using Ardalis.Result;
using MediatR;
using System.Threading;
using System.Threading.Tasks;
using Tutor.Application.Features.Tutors.Dto;
using Tutor.Application.Interfaces;

namespace Tutor.Application.Features.Tutors.Approve_Tutor;

public class ApproveTutorHandler : IRequestHandler<ApproveTutorCommand, Result<TutorProfileDto>>
{
    private readonly ITutorService _tutorService;

    public ApproveTutorHandler(ITutorService tutorService)
    {
        
        _tutorService = tutorService;   
    }
    
    public async Task<Result<TutorProfileDto>> Handle(ApproveTutorCommand request, CancellationToken cancellationToken)
    {
        return await _tutorService.ApproveTutorAsync(request.userId);
    }
}