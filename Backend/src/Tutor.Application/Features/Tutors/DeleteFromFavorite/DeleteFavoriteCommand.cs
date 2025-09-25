using Ardalis.Result;
using MediatR;

namespace Tutor.Application.Features.Tutors.DeleteFromFavorite;

public record DeleteFavoriteCommand(int UserId, int TutorUserId) : IRequest<Result<bool>>;