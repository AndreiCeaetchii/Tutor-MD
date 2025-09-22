using Ardalis.Result;
using MediatR;
using System.Collections.Generic;
using Tutor.Application.Features.Tutors.Dto;
using Tutor.Domain.Entities;

namespace Tutor.Application.Features.Tutors.CreateTutor;

public record CreateTutorCommand(int UserId, CreateTutorProfileDto createTutorProfileDto)
    : IRequest<Result<TutorProfileDto>>;
