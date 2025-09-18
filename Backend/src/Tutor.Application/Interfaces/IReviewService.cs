using Ardalis.Result;
using System.Threading.Tasks;
using Tutor.Application.Features.Reviews.Dto;

namespace Tutor.Application.Interfaces;

public interface IReviewService
{
    Task<Result<ReviewDto>> CreateReview(CreateReviewDto createDto, int studentUserId);
    Task<Result<ReviewDto>> UpdateReview(int reviewId, UpdateReviewDto updateDto, int studentUserId);
    Task<Result> DeleteReview(int reviewId, int studentUserId);
    Task<Result<TutorReviewDto>> GetReviewsForTutor(int tutorUserId,int? currentUserId = null);

    Task<Result<ReviewDto>> GetReviewForBooking(int bookingId, int studentUserId);
}