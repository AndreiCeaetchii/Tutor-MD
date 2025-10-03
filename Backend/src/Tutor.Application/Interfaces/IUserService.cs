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
    Task<Result> UpdateProfileAsync(int userId, CreateProfileDto profileDto);
    Task<Result> ActivateUserAsync(int userId);
    Task<Result> DeactivateUserAsync(int userId);
    Task<Result> CreateAdminAsync(int userId);
    Task<Result> RequestPasswordResetAsync(string email);
    Task<Result> ResetPasswordAsync(string token, string newPassword);
    Task Update(User user);
}