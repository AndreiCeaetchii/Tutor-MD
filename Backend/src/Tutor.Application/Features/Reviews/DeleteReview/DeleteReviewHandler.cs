using Ardalis.Result;
using MediatR;
using System.Threading;
using System.Threading.Tasks;
using Tutor.Application.Interfaces;

namespace Tutor.Application.Features.Reviews.DeleteReview;

public class DeleteReviewHandler : IRequestHandler<DeleteReviewCommand, Result>
{
    private readonly IReviewService _reviewService;

    public DeleteReviewHandler(IReviewService reviewService)
    {
        _reviewService = reviewService;
    }
    public async Task<Result> Handle(DeleteReviewCommand request, CancellationToken cancellationToken)
    {
        return await _reviewService.DeleteReview(request.ReviewId, request.UserId);
    }
}