using Ardalis.Result;
using MediatR;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Tutor.Application.Features.Tutors.Dto;
using Tutor.Application.Interfaces;

namespace Tutor.Application.Features.Tutors.GetAllTutorsForAdmin;

public class GetAllTutorsAdminHandler :  IRequestHandler<GetAllTutorsAdminCommand, Result<List<TutorProfileDto>>>
{
    private readonly ITutorService _tutorService;

    public GetAllTutorsAdminHandler(ITutorService tutorService)
    {
        _tutorService = tutorService;
    }
    public async Task<Result<List<TutorProfileDto>>> Handle(GetAllTutorsAdminCommand request, CancellationToken cancellationToken)
    {
        return await _tutorService.GetAllTutorProfileAsyncForAdmin();
    }
}