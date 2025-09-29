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
    private readonly IEmailService _emailService;
    private readonly IGenericRepository<User, int> _userRepository;
    private readonly IGenericRepository<GoogleAuth, int> _googleAuthRepository;
    private readonly IGenericRepository2<UserRole> _userRoleRepository;
    private readonly IPasswordHasher _passwordHasher;
    private readonly IGenericRepository2<Password> _passwordRepository;

    public UserService(IGenericRepository<User, int> userRepository,
        IGenericRepository<GoogleAuth, int> googleAuthRepository,
        IEmailService emailService,
        IGenericRepository2<UserRole> userRoleRepository,
        IGenericRepository2<Password> passwordRepository,
        IPasswordHasher passwordHasher)
    {
        _userRepository = userRepository;
        _googleAuthRepository = googleAuthRepository;
        _userRoleRepository = userRoleRepository;
        _emailService = emailService;
        _passwordHasher = passwordHasher;
        _passwordRepository = passwordRepository;
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

        if (!string.IsNullOrWhiteSpace(profileDto.Username))
        {
            var existingUser = await _userRepository.FindAsync(u => u.Username == profileDto.Username);
            if (existingUser != null && existingUser.Any())
            {
                var differentUser = existingUser.FirstOrDefault(u => u.Id != userId);
                if (differentUser != null)
                {
                    return Result.Error("Username is already taken");
                }
            }
        }

        user.Phone = profileDto.Phone;
        user.FirstName = profileDto.FirstName;
        user.LastName = profileDto.LastName;
        user.Bio = profileDto.Bio;
        user.Birthdate = profileDto.Birthdate;
        user.Username = profileDto.Username;
        user.City = profileDto.City;
        user.Country = profileDto.Country;

        await _userRepository.Update(user);

        return Result.Success();
    }

    public async Task Update(User user)
    {
        await _userRepository.Update(user);
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

    public async Task<Result> DeactivateUserAsync(int userId)
    {
        var user = await _userRepository.GetById(userId);
        if (user == null)
            return Result.Error("User not found");
        user.IsActive = false;
        await _userRepository.Update(user);
        return Result.Success();
    }

    public async Task<Result> ActivateUserAsync(int userId)
    {
        var user = await _userRepository.GetById(userId);
        if (user == null)
            return Result.Error("User not found");
        user.IsActive = true;
        await _userRepository.Update(user);
        return Result.Success();
    }

    public async Task<Result> CreateAdminAsync(int userId)
    {
        var user = await _userRepository.GetById(userId);
        if (user == null)
            return Result.Error("User not found");

        var currentUserRole = await _userRoleRepository.FindAsyncDefault(u => u.UserId == userId);

        await _userRoleRepository.Delete(currentUserRole);

        var userRole = new UserRole { UserId = user.Id, RoleId = 1, AssignedAt = DateTime.Now };
        await _userRoleRepository.Create(userRole);
        return Result.Success();
    }

    public async Task<Result> RequestPasswordResetAsync(string email)
    {
        var user = await GetUserByEmailAsync(email);
        if (user == null)
            return Result.Error("User not found with this email");
        var token = Guid.NewGuid().ToString("N");

        user.ResetToken = token;
        user.ResetTokenExpiresAt = DateTime.UtcNow.AddHours(1);

        await _userRepository.Update(user);

        var resetLink = $"http://localhost:5173/reset-password?token={token}";
        var htmlMessage = $@"
<!DOCTYPE html>
<html>
<head>
    <meta charset='UTF-8'>
    <title>Reset Your Tutor Platform Password</title>
</head>
<body style='font-family: Arial, sans-serif; line-height: 1.6;'>
    <p>Hi {user.FirstName},</p>

    <p>We received a request to reset your password for your Tutor Platform account. Click the button below to create a new password:</p>

    <p>
        <a href='{resetLink}' style='background-color: #5F3AEB; color: white; padding: 12px 24px; text-decoration: none; border-radius: 5px; display: inline-block;'>Reset Password</a>
    </p>

    <p>This link will expire in 24 hours for your security. If you did not request a password reset, you can safely ignore this email.</p>

    <p>Thank you,<br/>Tutor Team</p>
</body>
</html>
";
        var result = await _emailService.SendEmailAsync(user.Email, "Password Reset", htmlMessage);
        if (result == false)
            return Result.Error("Something went wrong");
        return Result.Success();
    }

    public async Task<Result> ResetPasswordAsync(string token, string newPassword)
    {
        var user = await _userRepository.FindAsyncDefault(t =>
            t.ResetToken == token && t.ResetTokenExpiresAt > DateTime.UtcNow);

        if (user == null)
            return Result.Error("Invalid or expired token");
        user.ResetToken = string.Empty;
        user.ResetTokenExpiresAt = default;
        var password = await _passwordRepository.FindAsyncDefault(u => u.UserId == user.Id);
        var newPasswordHash = _passwordHasher.HashPassword(newPassword);
        password.PasswordHash = newPasswordHash;
        await _passwordRepository.Update(password);
        await _userRepository.Update(user);


        return Result.Success();
    }
}