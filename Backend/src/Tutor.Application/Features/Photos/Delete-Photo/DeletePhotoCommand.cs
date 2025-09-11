using Ardalis.Result;
using MediatR;
using Tutor.Application.Features.Photos.DTOs;

namespace Tutor.Application.Features.Photos.Delete_Photo;

public record DeletePhotoCommand(int userId): IRequest<Result>;
