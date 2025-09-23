using Ardalis.Result;
using MediatR;
using System.Threading;
using System.Threading.Tasks;
using Tutor.Application.Features.Reviews.Dto;
using Tutor.Application.Interfaces;

namespace Tutor.Application.Features.Reviews.CreateReview;

public class CreateReviewHandler : IRequestHandler<CreateReviewCommand, Result<ReviewDto>>
{
    private readonly IReviewService  _reviewService;

    public CreateReviewHandler(IReviewService reviewService)
    {
        _reviewService = reviewService;
    }
    public async Task<Result<ReviewDto>> Handle(CreateReviewCommand request, CancellationToken cancellationToken)
    {
        return await _reviewService.CreateReview(request.Review,request.UserId);
    }
}