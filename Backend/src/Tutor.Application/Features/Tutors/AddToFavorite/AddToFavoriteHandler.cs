using Ardalis.Result;
using MediatR;
using System.Threading;
using System.Threading.Tasks;
using Tutor.Application.Interfaces;

namespace Tutor.Application.Features.Tutors.AddToFavorite;

public class AddToFavoriteHandler : IRequestHandler<AddToFavoriteCommand, Result<bool>>
{
    private readonly ITutorService  _tutorService;

    public AddToFavoriteHandler(ITutorService tutorService)
    {
        _tutorService = tutorService;
    }
    public async Task<Result<bool>> Handle(AddToFavoriteCommand request, CancellationToken cancellationToken)
    {
        return await _tutorService.AddToFavorite(request.UserId, request.TutorUserId);
    }
}