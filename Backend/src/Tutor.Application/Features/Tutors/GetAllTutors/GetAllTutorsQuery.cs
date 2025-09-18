using Ardalis.Result;
using MediatR;
using System.Collections.Generic;
using Tutor.Application.Features.Tutors.Dto;

namespace Tutor.Application.Features.Tutors.GetAllTutors;

public record GetAllTutorsQuery()
    : IRequest<Result<List<TutorProfileDto>>>;
