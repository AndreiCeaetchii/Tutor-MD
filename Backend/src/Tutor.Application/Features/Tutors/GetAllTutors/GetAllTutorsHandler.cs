using Ardalis.Result;
using MediatR;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Tutor.Application.Features.Tutors.Dto;
using Tutor.Application.Interfaces;

namespace Tutor.Application.Features.Tutors.GetAllTutors;

public class GetAllTutorsHandler : IRequestHandler<GetAllTutorsQuery, Result<List<TutorProfileDto>>>
{
    private readonly ITutorService  _tutorService;
   
    public GetAllTutorsHandler(
        ITutorService tutorService
    )
    {
        _tutorService = tutorService;
    }

    
    public async Task<Result<List<TutorProfileDto>>> Handle(GetAllTutorsQuery request, CancellationToken cancellationToken)
    {
        return await _tutorService.GetAllTutorProfileAsync(
            request.City,
            request.Country,
            request.SubjectId,
            request.Ratings,
            request.MinPrice,
            request.MaxPrice,
            request.SortBy,
            request.SortDescending
        );
    }
}