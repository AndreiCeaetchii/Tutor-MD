using Ardalis.Result;
using MediatR;
using Tutor.Application.Features.Reviews.Dto;

namespace Tutor.Application.Features.Reviews.GetReviews;

public record GetReviewsCommand(int TutorId, int UserId): IRequest<Result<TutorReviewDto>> ;