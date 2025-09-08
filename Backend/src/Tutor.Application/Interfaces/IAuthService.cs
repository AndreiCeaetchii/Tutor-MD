using Ardalis.Result;
using System.Threading.Tasks;
using Tutor.Application.Features.Users.Dtos;

namespace Tutor.Application.Interfaces;

public interface IAuthService
{
    Task<Result<UserResponseDto>> LoginAsync(LoginUserDto loginDto);   
    Task<Result<UserResponseDto>> RegisterAsync(RegisterUserDto registerDto);

}