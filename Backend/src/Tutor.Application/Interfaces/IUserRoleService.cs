using Ardalis.Result;
using System.Threading.Tasks;

namespace Tutor.Application.Interfaces;

public interface IUserRoleService
{
    Task<Result<bool>> AssignTutorRoleAsync(int userId);
    Task<bool> HasRoleAsync(int userId, int roleId);
    Task<bool> HasAnyRoleAsync(int userId);
}