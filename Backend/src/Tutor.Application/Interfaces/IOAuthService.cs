using System.Threading.Tasks;
using Tutor.Application.Features.Users.Dtos;
using Tutor.Domain.Entities;

namespace Tutor.Application.Interfaces;

public interface IOAuthService
{
    Task<OAuthUserInfo> ValidateGoogleTokenAsync(string accessToken);

}