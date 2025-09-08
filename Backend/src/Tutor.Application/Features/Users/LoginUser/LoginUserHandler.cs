using Ardalis.Result;
using MediatR;
using System.Threading;
using System.Threading.Tasks;
using Tutor.Application.Features.Users.Dtos;
using Tutor.Application.Interfaces;
using Tutor.Domain.Entities;
using Tutor.Domain.Interfaces;

namespace Tutor.Application.Features.Users.LoginUser;

public class LoginUserCommandHandler : IRequestHandler<LoginUserCommand, Result<UserResponseDto>>
{
    private readonly IGenericRepository<User, int> _userRepository;
    private readonly IPasswordHasher _passwordHasher;
    private readonly ITokenService _tokenService;

    public LoginUserCommandHandler(
        IGenericRepository<User, int> userRepository,
        IPasswordHasher passwordHasher,
        ITokenService tokenService)
    {
        _userRepository = userRepository;
        _passwordHasher = passwordHasher;
        _tokenService = tokenService;
    }

    public async Task<Result<UserResponseDto>> Handle(LoginUserCommand request, CancellationToken cancellationToken)
    {
        var loginDto = request.LoginUserDto;

        // Check if user already exists
        var existingUser = await _userRepository.FindAsyncDefault(u =>
            u.Email == loginDto.Email);

        if (existingUser == null)
        {
            return Result<UserResponseDto>.Error("Invalid Email");
        }

        var token = _tokenService.GenerateToken(existingUser);

        if (_passwordHasher.VerifyPassword(loginDto.Password, existingUser.PasswordHash))
        {
            var response = new UserResponseDto
            {
                Id = existingUser.Id, // auto-assigned by DB
                Email = existingUser.Email,
                Username = existingUser.Username,
                Token = token,
            };

            return Result<UserResponseDto>.Success(response);
        } 
        return Result<UserResponseDto>.Error("Invalid Password");
        



    }
}