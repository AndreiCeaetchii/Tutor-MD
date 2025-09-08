using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Tutor.Application.Interfaces;
using Tutor.Domain.Entities;
using Tutor.Domain.Interfaces;

namespace Tutor.Application.Services;

public class UserService : IUserService
{
    private readonly IGenericRepository<User, int> _userRepository;

    public UserService(IGenericRepository<User, int> userRepository)
    {
        _userRepository = userRepository;
    }

    public async Task<User?> GetUserByOAuthIdAsync(string provider, string providerId)
    {
        Expression<Func<User, bool>> predicate = u => 
            u.OAuthProvider == provider && u.OAuthProviderId == providerId;
        
        return await _userRepository.FindAsyncDefault(predicate);
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
        string email)
    {
        var user = new User
        {
            Email = email,
            OAuthProvider = provider,
            OAuthProviderId = providerId,
            IsActive = true
        };

        await _userRepository.Create(user);
        return user;
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

    public async Task<bool> UserExistsByOAuthIdAsync(string provider, string providerId)
    {
        Expression<Func<User, bool>> predicate = u => 
            u.OAuthProvider == provider && u.OAuthProviderId == providerId;
        
        var user = await _userRepository.FindAsyncDefault(predicate);
        return user != null;
    }
}