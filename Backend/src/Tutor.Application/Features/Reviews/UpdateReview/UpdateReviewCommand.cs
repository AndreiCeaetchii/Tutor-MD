using Ardalis.Result;
using MediatR;
using Tutor.Application.Features.Reviews.Dto;

namespace Tutor.Application.Features.Reviews.UpdateReview;

public record UpdateReviewCommand(int UserId, UpdateReviewDto Review, int ReviewId) :IRequest<Result<ReviewDto>>;