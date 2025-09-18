using Ardalis.Result;
using MediatR;
using System.Threading;
using System.Threading.Tasks;
using Tutor.Application.Features.Reviews.Dto;
using Tutor.Application.Interfaces;

namespace Tutor.Application.Features.Reviews.GetReviews;

public class GetReviewsHandler : IRequestHandler<GetReviewsCommand, Result<TutorReviewDto>>
{
    private readonly IReviewService _reviewService;

    public GetReviewsHandler(IReviewService reviewService)
    {
        _reviewService = reviewService;
    }
    public async Task<Result<TutorReviewDto>> Handle(GetReviewsCommand request, CancellationToken cancellationToken)
    {
        return await _reviewService.GetReviewsForTutor(request.TutorId, request.UserId);
    }
} 