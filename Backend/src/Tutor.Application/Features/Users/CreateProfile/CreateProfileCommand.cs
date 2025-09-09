using Ardalis.Result;
using MediatR;
using Tutor.Application.Features.Users.Dtos;

namespace Tutor.Application.Features.Users.CreateProfile;

public record CreateProfileCommand(int UserId, CreateProfileDto ProfileDto) : IRequest<Result<CreateProfileDto>>;