using Ardalis.Result;
using MediatR;

namespace Tutor.Application.Features.Tutors.AddToFavorite;

public record AddToFavoriteCommand(int UserId, int TutorUserId) : IRequest<Result<bool>>;