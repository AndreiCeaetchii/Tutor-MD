using Ardalis.Result;
using System;
using System.Threading.Tasks;
using Tutor.Application.Interfaces;
using Tutor.Domain.Entities;
using Tutor.Domain.Interfaces;

namespace Tutor.Application.Services;

public class UserRoleService : IUserRoleService
{
    private readonly IGenericRepository2<UserRole> _roleRepository;

    public UserRoleService(IGenericRepository2<UserRole> roleRepository)
    {
        _roleRepository = roleRepository;
    }
    

    public async Task<bool> HasRoleAsync(int userId, int roleId) =>
        await _roleRepository.FindAsyncDefault(ur => ur.UserId == userId && ur.RoleId == roleId) != null;

    public async Task<bool> HasAnyRoleAsync(int userId) =>
        await _roleRepository.FindAsyncDefault(ur => ur.UserId == userId) != null;
    
    public async Task<int?> GetRoleIdAsync(int userId)
    {
        var role = await _roleRepository.FindAsyncDefault(ur => ur.UserId == userId);
        return role?.RoleId; // returns null if no role found
    }

}