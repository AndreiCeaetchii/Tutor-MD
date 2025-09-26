using Ardalis.Result;
using MediatR;
using System.Threading;
using System.Threading.Tasks;
using Tutor.Application.Interfaces;

namespace Tutor.Application.Features.Tutors.DeleteFromFavorite;

public class DeleteFavoriteHandler : IRequestHandler<DeleteFavoriteCommand,Result<bool>>
{
    private readonly ITutorService _tutorService;

    public DeleteFavoriteHandler(ITutorService tutorService)
    {
        _tutorService = tutorService;
    }
    public async Task<Result<bool>> Handle(DeleteFavoriteCommand request, CancellationToken cancellationToken)
    {
        return await _tutorService.DeleteFavorite(request.UserId, request.TutorUserId);
    }
}