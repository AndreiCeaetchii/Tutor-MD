using Ardalis.Result;
using System;
using System.Threading.Tasks;
using Tutor.Application.Features.Users.Dtos;
using Tutor.Domain.Entities;

namespace Tutor.Application.Interfaces;

public interface IUserService
{
    Task<User?> GetUserByOAuthIdAsync(string provider, string providerId);
    Task<User?> GetUserByEmailAsync(string email);
    Task<User> CreateUserFromOAuthAsync(string provider, string providerId, string email);
    
    Task<Result<CreateProfileDto>> UpdateProfileAsync(int userId, CreateProfileDto profileDto);

}
