using Ardalis.Result;
using MediatR;
using System.Threading;
using System.Threading.Tasks;
using Tutor.Application.Features.Photos.DTOs;
using Tutor.Application.Interfaces;

namespace Tutor.Application.Features.Photos.Add_Photo;

public class AddPhotoHandler : IRequestHandler<AddPhotoCommand, Result<PhotoDto>>
{
    private readonly IPhotoService _photoService;

    public AddPhotoHandler(IPhotoService photoService)
    {
        _photoService = photoService;
    }
    public async Task<Result<PhotoDto>> Handle(AddPhotoCommand request, CancellationToken cancellationToken)
    {
        return await _photoService.AddPhotoAsync(request.file, request.userId);
    }
}