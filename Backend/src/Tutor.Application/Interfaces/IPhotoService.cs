using Ardalis.Result;
using CloudinaryDotNet.Actions;
using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;
using Tutor.Application.Features.Photos.DTOs;

namespace Tutor.Application.Interfaces;

public interface IPhotoService
{
    Task<ImageUploadResult> UploadPhotoAsync(IFormFile file);
    Task<DeletionResult> DeletePhotoAsync(string publicId);

    Task<Result<PhotoDto>> AddPhotoAsync(IFormFile file, int userId);
}