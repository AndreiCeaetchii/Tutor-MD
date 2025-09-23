using Ardalis.Result;
using System.Threading.Tasks;

namespace Tutor.Application.Interfaces;

public interface IUserRoleService
{
    Task<bool> HasRoleAsync(int userId, int roleId);
    Task<bool> HasAnyRoleAsync(int userId);
    Task<int?> GetRoleIdAsync(int userId);
}