using Ardalis.Result;
using MediatR;
using System.Threading;
using System.Threading.Tasks;
using Tutor.Application.Features.Users.Dtos;
using Tutor.Application.Interfaces;
using Tutor.Domain.Entities;
using Tutor.Domain.Interfaces;

namespace Tutor.Application.Features.Users.RegisterUser;

public class RegisterUserCommandHandler : IRequestHandler<RegisterUserCommand, Result<UserResponseDto>>
{
    private readonly IGenericRepository<User, int> _userRepository;  
    private readonly IPasswordHasher _passwordHasher;
    private readonly ITokenService _tokenService;

    public RegisterUserCommandHandler(
        IGenericRepository<User, int> userRepository,
        IPasswordHasher passwordHasher,
        ITokenService tokenService)
    {
        _userRepository = userRepository;
        _passwordHasher = passwordHasher;
        _tokenService = tokenService;
    }

    public async Task<Result<UserResponseDto>> Handle(RegisterUserCommand request, CancellationToken cancellationToken)
    {
        var registerDto = request.RegisterUserDto;

      

        // Hash password (salt is embedded internally)
        var hashedPassword = _passwordHasher.HashPassword(registerDto.Password);

        // Map DTO to User entity using the mapper
        var user = UserMapper.ToEntity(registerDto, hashedPassword);

        // Add user to database
        await _userRepository.Create(user);
        
        var token = _tokenService.GenerateToken(user);

        // Map User entity to response DTO
        var response = new UserResponseDto
        {
            Id = user.Id, // auto-assigned by DB
            Email = user.Email,
            Token = token
        };

        return Result<UserResponseDto>.Success(response);
    }
}
