using Ardalis.Result;
using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
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
    private readonly IUserService _userService;

    public CreateTutorCommandHandler(
        ITutorService tutorService,
        IUserService userService
    )
    {
        _tutorService = tutorService;
        _userService = userService;
    }

    public async Task<Result<TutorProfileDto>> Handle(CreateTutorCommand request, CancellationToken cancellationToken)
    {
        var result = await _userService.UpdateProfileAsync(request.UserId, request.createTutorProfileDto.CreateProfileDto);
        if (result.Errors.Any())
            return  Result<TutorProfileDto>.Error(result.Errors.ToString());
        return await _tutorService.CreateTutorProfileAsync(request.createTutorProfileDto, request.UserId);
    }
}