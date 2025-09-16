using Ardalis.Result;
using MediatR;
using Microsoft.AspNetCore.Http;
using Tutor.Application.Features.Photos.DTOs;

namespace Tutor.Application.Features.Photos.Add_Photo;

public record AddPhotoCommand(int userId, IFormFile file) : IRequest<Result<PhotoDto>>;
