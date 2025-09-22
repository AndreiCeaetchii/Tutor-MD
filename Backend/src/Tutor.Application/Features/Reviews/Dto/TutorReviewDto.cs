using System.Collections.Generic;

namespace Tutor.Application.Features.Reviews.Dto;

public class TutorReviewDto
{
    public List<ReviewDto>  Reviews { get; set; }
    public int? AverageRating { get; set; }
}