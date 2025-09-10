using Ardalis.Result;
using MediatR;
using System.Threading;
using System.Threading.Tasks;
using Tutor.Application.Features.Tutors.Dto;
using Tutor.Application.Interfaces;

namespace Tutor.Application.Features.Tutors.GetTutorById;

public class GetTutorByIdHandler : IRequestHandler<GetTutorByIdQuery, Result<TutorProfileDto>>
{
    private readonly ITutorService  _tutorService;
   
    public GetTutorByIdHandler(
        ITutorService tutorService
    )
    {
        _tutorService = tutorService;
    }

    public async Task<Result<TutorProfileDto>> Handle(GetTutorByIdQuery request, CancellationToken cancellationToken)
    {
        return await _tutorService.GetTutorProfileAsync(request.UserId);
    }

    
}