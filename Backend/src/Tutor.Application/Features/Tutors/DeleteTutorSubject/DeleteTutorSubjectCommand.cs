using Ardalis.Result;
using MediatR;
using System.Collections.Generic;
using Tutor.Application.Features.Tutors.Dto;

namespace Tutor.Application.Features.Tutors.DeleteTutorSubject;

public record DeleteTutorSubjectCommand(int userId, int subjectId) : IRequest<Result<List<TutorSubjectDto>>>;
