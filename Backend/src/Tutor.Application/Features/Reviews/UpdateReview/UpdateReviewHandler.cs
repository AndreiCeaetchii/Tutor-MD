using Ardalis.Result;
using MediatR;
using System.Threading;
using System.Threading.Tasks;
using Tutor.Application.Features.Reviews.Dto;
using Tutor.Application.Interfaces;

namespace Tutor.Application.Features.Reviews.UpdateReview;

public class UpdateReviewHandler :  IRequestHandler<UpdateReviewCommand, Result<ReviewDto>>
{
    private readonly IReviewService _reviewService;

    public UpdateReviewHandler(IReviewService reviewService)
    {
        _reviewService = reviewService;
    }
    public async Task<Result<ReviewDto>> Handle(UpdateReviewCommand request, CancellationToken cancellationToken)
    {
        return await _reviewService.UpdateReview(request.ReviewId, request.Review, request.UserId);
    }
}