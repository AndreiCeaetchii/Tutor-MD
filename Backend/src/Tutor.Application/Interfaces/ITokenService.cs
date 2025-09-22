using Tutor.Domain.Entities;

namespace Tutor.Application.Interfaces;

public interface ITokenService
{
    public string GenerateToken(User user);
}