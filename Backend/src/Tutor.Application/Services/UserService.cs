using Ardalis.Result;
using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Tutor.Application.Features.Users.Dtos;
using Tutor.Application.Interfaces;
using Tutor.Domain.Entities;
using Tutor.Domain.Interfaces;

namespace Tutor.Application.Services;

public class UserService : IUserService
{
    private readonly IGenericRepository<User, int> _userRepository;
    private readonly IGenericRepository<GoogleAuth, int> _googleAuthRepository;

    public UserService(IGenericRepository<User, int> userRepository,
        IGenericRepository<GoogleAuth, int> googleAuthRepository)
    {
        _userRepository = userRepository;
        _googleAuthRepository = googleAuthRepository;
    }

    public async Task<User?> GetUserByOAuthIdAsync(string provider, string providerId)
    {
        // Validate input parameters
        if (string.IsNullOrEmpty(provider) || string.IsNullOrEmpty(providerId))
            return null;
        Expression<Func<GoogleAuth, bool>> predicate = u =>
            u.OAuthProvider == provider && u.OAuthProviderId == providerId;

        var googleAuth = await _googleAuthRepository.FindAsyncDefault(predicate);

        if (googleAuth == null)
            return null;
        return await _userRepository.GetById(googleAuth.UserId);
    }

    public async Task<User?> GetUserByEmailAsync(string email)
    {
        Expression<Func<User, bool>> predicate = u =>
            u.Email == email;

        return await _userRepository.FindAsyncDefault(predicate);
    }

    public async Task<User> CreateUserFromOAuthAsync(
        string provider,
        string providerId,
        string email
    )
    {
        var user = new User { Email = email, IsActive = true, LastLoginAt = DateTime.UtcNow, };


        await _userRepository.Create(user);

        var googleAuth = new GoogleAuth { UserId = user.Id, OAuthProvider = provider, OAuthProviderId = providerId, };

        await _googleAuthRepository.Create(googleAuth);
        return user;
    }

    public async Task<Result> UpdateProfileAsync(int userId, CreateProfileDto profileDto)
    {
        var user = await _userRepository.GetById(userId);

        if (user == null)
            return Result.Error("User not found");

        // Update fields
        user.Phone = profileDto.Phone;
        user.FirstName = profileDto.FirstName;
        user.LastName = profileDto.LastName;
        user.Bio = profileDto.Bio;
        user.Birthdate = profileDto.Birthdate;
        user.Username = profileDto.Username;

        await _userRepository.Update(user);

        return Result.Success();
    }

    public async Task UpdateLastLoginAsync(int userId)
    {
        var user = await _userRepository.GetById(userId);
        if (user != null)
        {
            user.LastLoginAt = DateTime.UtcNow;
            await _userRepository.Update(user);
        }
    }

    // Additional useful methods
    public async Task<bool> UserExistsByEmailAsync(string email)
    {
        Expression<Func<User, bool>> predicate = u =>
            u.Email == email;

        var user = await _userRepository.FindAsyncDefault(predicate);
        return user != null;
    }

    // public async Task<bool> UserExistsByOAuthIdAsync(string provider, string providerId)
    // {
    //     Expression<Func<User, bool>> predicate = u => 
    //         u.OAuthProvider == provider && u.OAuthProviderId == providerId;
    //     
    //     var user = await _userRepository.FindAsyncDefault(predicate);
    //     return user != null;
    // }
}