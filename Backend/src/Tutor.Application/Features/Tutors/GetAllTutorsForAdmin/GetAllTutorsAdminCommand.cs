using Ardalis.Result;
using MediatR;
using System.Collections.Generic;
using Tutor.Application.Features.Tutors.Dto;

namespace Tutor.Application.Features.Tutors.GetAllTutorsForAdmin;

public record GetAllTutorsAdminCommand(): IRequest<Result<List<TutorProfileDto>>> ;