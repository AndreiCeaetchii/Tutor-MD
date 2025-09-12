using Ardalis.Result;
using MediatR;
using System.Threading;
using System.Threading.Tasks;
using Tutor.Application.Features.Photos.DTOs;
using Tutor.Application.Interfaces;

namespace Tutor.Application.Features.Photos.Delete_Photo;

public class DeletePhotoHandler : IRequestHandler<DeletePhotoCommand, Result>
{
    private readonly IPhotoService _photoService;

    public DeletePhotoHandler(IPhotoService photoService)
    {
        _photoService = photoService;
    }
    public async Task<Result> Handle(DeletePhotoCommand request, CancellationToken cancellationToken)
    {
       return await _photoService.DeletePhotoEntityAsync(request.userId);
    }
}