using Ardalis.Result;
using MediatR;
using Tutor.Application.Features.Reviews.Dto;

namespace Tutor.Application.Features.Reviews.CreateReview;

public record CreateReviewCommand(int UserId, CreateReviewDto Review) : IRequest<Result<ReviewDto>>;