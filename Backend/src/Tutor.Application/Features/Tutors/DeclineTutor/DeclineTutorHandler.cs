using Ardalis.Result;
using MediatR;
using System.Threading;
using System.Threading.Tasks;
using Tutor.Application.Features.Tutors.Dto;
using Tutor.Application.Interfaces;

namespace Tutor.Application.Features.Tutors.DeclineTutor;

public class DeclineTutorHandler : IRequestHandler<DeclineTutorCommand, Result<TutorProfileDto>>
{
    private readonly ITutorService _tutorService;

    public DeclineTutorHandler(ITutorService tutorService)
    {
        
        _tutorService = tutorService;   
    }
    
    public async Task<Result<TutorProfileDto>> Handle(DeclineTutorCommand request, CancellationToken cancellationToken)
    {
        return await _tutorService.DeclineTutorAsync(request.userId);
    }
}
