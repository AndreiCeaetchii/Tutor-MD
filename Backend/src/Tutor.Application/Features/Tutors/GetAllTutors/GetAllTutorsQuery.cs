using Ardalis.Result;
using MediatR;
using System.Collections.Generic;
using Tutor.Application.Features.Tutors.Dto;

namespace Tutor.Application.Features.Tutors.GetAllTutors;

public record GetAllTutorsQuery(
    int UserId,
    string? City = null,
    string? Country = null,
    int[]? SubjectId= null,
    int[]? Ratings= null,
    decimal? MinPrice = null,
    decimal? MaxPrice = null,
    string? SortBy = null,
    bool SortDescending = false
) : IRequest<Result<List<TutorProfileDto>>>;