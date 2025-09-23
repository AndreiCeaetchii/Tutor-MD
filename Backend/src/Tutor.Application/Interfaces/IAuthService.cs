using Ardalis.Result;
using System.Threading.Tasks;
using Tutor.Application.Features.Users.Dtos;

namespace Tutor.Application.Interfaces;

public interface IAuthService
{
    Task<Result<UserResponseDto>> LoginAsync(LoginUserDto loginDto,string mfaCode = null);
    Task<Result<UserResponseDto>> RegisterAsync(RegisterUserDto registerDto);
    Task<Result<EnableMFAResponse>> EnableMFAAsync(int userId);
    Task<bool> VerifyMFAAsync(int userId, string code);
    Task<bool> DisableMFAAsync(int userId);
}