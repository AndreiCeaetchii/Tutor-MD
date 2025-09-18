using Ardalis.Result;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;
using Tutor.Application.Features.Reviews.Dto;
using Tutor.Application.Interfaces;
using Tutor.Domain.Entities;
using Tutor.Domain.Interfaces;

namespace Tutor.Application.Services;

public class ReviewService : IReviewService
{
    private readonly IGenericRepository<Review, int> _reviewRepository;
    private readonly IGenericRepository<Booking, int> _bookingRepository;

    public ReviewService(
        IGenericRepository<Review, int> reviewRepository,
        IGenericRepository<Booking, int> bookingRepository)
    {
        _reviewRepository = reviewRepository;
        _bookingRepository = bookingRepository;
    }

    public async Task<Result<ReviewDto>> CreateReview(CreateReviewDto createDto, int studentUserId)
    {
        var booking = await _bookingRepository.GetById(createDto.BookingId);
        if (booking == null)
            return Result.NotFound("Booking not found, unable to create review");

        if (booking.StudentUserId != studentUserId)
            return Result.Forbidden("You can only review your own bookings");

        var existingReview = await _reviewRepository.FindAsync(r => r.TutorUserId == booking.TutorUserId);
        if (existingReview.Any())
            return Result.Conflict("Review already exists for this tutor");

        if (createDto.Rating < 1 || createDto.Rating > 5)
            return Result.Error("Rating must be between 1 and 5");

        var review = new Review
        {
            BookingId = createDto.BookingId,
            TutorUserId = booking.TutorUserId,
            StudentUserId = studentUserId,
            Rating = createDto.Rating,
            Description = createDto.Description,
            CreatedAt = DateTime.UtcNow
        };

        await _reviewRepository.Create(review);

        return Result.Success(MapToDto(review));
    }

    public async Task<Result<ReviewDto>> UpdateReview(int reviewId, UpdateReviewDto updateDto, int studentUserId)
    {
        var review = await _reviewRepository.GetById(reviewId);
        if (review == null)
            return Result.NotFound("Review not found");

        if (review.StudentUserId != studentUserId)
            return Result.Forbidden("You can only update your own reviews");

        if (updateDto.Rating < 1 || updateDto.Rating > 5)
            return Result.Error("Rating must be between 1 and 5");
        if (updateDto.Rating.HasValue)
            review.Rating = updateDto.Rating.Value;
        
        if(updateDto.Description is not null)
            review.Description = updateDto.Description;
        
        review.UpdatedAt = DateTime.UtcNow;

        await _reviewRepository.Update(review);

        return Result.Success(MapToDto(review));
    }

    public async Task<Result> DeleteReview(int reviewId, int studentUserId)
    {
        var review = await _reviewRepository.GetById(reviewId);
        if (review == null)
            return Result.NotFound("Review not found");

        if (review.StudentUserId != studentUserId)
            return Result.Forbidden("You can only delete your own reviews");

        await _reviewRepository.Delete(review);

        return Result.Success();
    }

    public async Task<Result<TutorReviewDto>> GetReviewsForTutor(int tutorUserId, int? currentUserId = null)
    {
        var reviews = await _reviewRepository.FindAsync(r => r.TutorUserId == tutorUserId);

        var reviewDtos = reviews
            .OrderByDescending(r => currentUserId.HasValue && r.StudentUserId == currentUserId.Value)
            .ThenByDescending(r => r.CreatedAt)
            .Select(MapToDto)
            .ToList();

        var averageRating = reviews.Any()
            ? (int?)reviews.Average(r => r.Rating)
            : null;

        return Result.Success(new TutorReviewDto { Reviews = reviewDtos, AverageRating = averageRating });
    }

    public async Task<Result<ReviewDto>> GetReviewForBooking(int bookingId, int studentUserId)
    {
        var review = await _reviewRepository.FindAsync(r => r.BookingId == bookingId);

        var reviewEntity = review.FirstOrDefault();
        if (reviewEntity == null)
            return Result.NotFound("Review not found for this booking");

        if (reviewEntity.StudentUserId != studentUserId)
            return Result.Forbidden("You can only view your own reviews");

        return Result.Success(MapToDto(reviewEntity));
    }

    private ReviewDto MapToDto(Review review)
    {
        string userName = "Unknown User";

        if (review.Student != null && review.Student.User != null)
        {
            userName = $"{review.Student.User.FirstName} {review.Student.User.LastName}".Trim();
            if (string.IsNullOrEmpty(userName))
            {
                userName = review.Student.User.Email ?? "Unknown User";
            }
        }

        return new ReviewDto
        {
            Id = review.Id,
            StudentUserId = review.StudentUserId,
            UserName = review.Student?.User?.FirstName + " " + review.Student?.User?.LastName,
            Rating = review.Rating,
            Description = review.Description,
            CreatedAt = review.CreatedAt
        };
    }
}