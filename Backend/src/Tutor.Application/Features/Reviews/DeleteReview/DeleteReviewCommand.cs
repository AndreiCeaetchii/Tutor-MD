using Ardalis.Result;
using MediatR;

namespace Tutor.Application.Features.Reviews.DeleteReview;

public record DeleteReviewCommand(int ReviewId, int UserId) : IRequest<Result>;