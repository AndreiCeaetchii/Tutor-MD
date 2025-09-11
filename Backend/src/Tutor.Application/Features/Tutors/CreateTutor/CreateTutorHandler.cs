using Ardalis.Result;
using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Tutor.Application.Features.Tutors.Dto;
using Tutor.Application.Interfaces;
using Tutor.Domain.Entities;
using Tutor.Domain.Interfaces;

namespace Tutor.Application.Features.Tutors.CreateTutor;

public class CreateTutorCommandHandler : IRequestHandler<CreateTutorCommand, Result<TutorProfileDto>>
{
    private readonly ITutorService _tutorService;

    public CreateTutorCommandHandler(
        ITutorService tutorService
    )
    {
        _tutorService = tutorService;
    }

    public async Task<Result<TutorProfileDto>> Handle(CreateTutorCommand request, CancellationToken cancellationToken)
    {
        return await _tutorService.CreateTutorProfileAsync(request.createTutorProfileDto, request.UserId);
    }
}