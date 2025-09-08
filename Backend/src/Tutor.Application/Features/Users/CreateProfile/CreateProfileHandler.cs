using Ardalis.Result;
using MediatR;
using System.Threading;
using System.Threading.Tasks;
using Tutor.Application.Features.Users.Dtos;
using Tutor.Application.Interfaces;
using Tutor.Domain.Entities;
using Tutor.Domain.Interfaces;

namespace Tutor.Application.Features.Users.CreateProfile;

    public class UpdateProfileCommandHandler : IRequestHandler<CreateProfileCommand, Result<CreateProfileDto>>
    {
        private readonly IUserService _userService;

        public UpdateProfileCommandHandler(IUserService userService)
        {
            _userService = userService;
        }

        public async Task<Result<CreateProfileDto>> Handle(CreateProfileCommand request, CancellationToken cancellationToken)
        {
            // Delegate profile update to service
            return await _userService.UpdateProfileAsync(request.UserId, request.ProfileDto);
        }
    }
