using Ardalis.Result;
using AutoMapper;
using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using System.Threading.Tasks;
using Tutor.Application.Features.Photos.DTOs;
using Tutor.Application.Interfaces;
using Tutor.Domain.Entities;
using Tutor.Domain.Interfaces;
using Tutor.Infrastructure.Helpers;

namespace Tutor.Infrastructure.Services;

public class PhotoService : IPhotoService
{
    private readonly IGenericRepository<Photo, int> _photoRepository;
    private readonly IGenericRepository<User, int> _userRepository;
    IMapper _mapper;
    private readonly Cloudinary _cloudinary;

    public PhotoService(IOptions<CloudinarySettings> config, IGenericRepository<Photo, int> photoRepository,
        IGenericRepository<User, int> userRepository, IMapper mapper)
    {
        var account = new Account(config.Value.CloudName, config.Value.ApiKey, config.Value.ApiSecret);

        _cloudinary = new Cloudinary(account);
        _photoRepository = photoRepository;
        _userRepository = userRepository;
        _mapper = mapper;
    }

    public async Task<ImageUploadResult> UploadPhotoAsync(IFormFile file)
    {
        var uploadResult = new ImageUploadResult();

        if (file.Length > 0)
        {
            await using var stream = file.OpenReadStream();
            var uploadParams = new ImageUploadParams()
            {
                File = new FileDescription(file.FileName, stream), Folder = "da-ang20"
            };
            uploadResult = await _cloudinary.UploadAsync(uploadParams);
        }

        return uploadResult;
    }

    public async Task<DeletionResult> DeletePhotoAsync(string publicId)
    {
        var deleteParams = new DeletionParams(publicId);
        return await _cloudinary.DestroyAsync(deleteParams);
    }

    public async Task<Result<PhotoDto>> AddPhotoAsync(IFormFile file, int userId)
    {
        var user = await _userRepository.GetById(userId);
        if (user == null)
            return  Result<PhotoDto>.Error("Inexistent User");
        if(user.PhotoId != null)
            return Result<PhotoDto>.Error("You cannot add more than one photo");
        
        var result = await UploadPhotoAsync(file);
        var photo = new Photo
        {
            Url = result.SecureUrl.AbsoluteUri,
            PublicId = result.PublicId,
            MimeType = file.ContentType,
            Provider = "cloudinary",
            Width = result.Width,
            Height = result.Height,
            Bytes = result.Bytes,
        };
        await _photoRepository.Create(photo);
        
        user.PhotoId = photo.Id;
        await _userRepository.Update(user);
        return Result<PhotoDto>.Success(_mapper.Map<PhotoDto>(photo));
    }
}