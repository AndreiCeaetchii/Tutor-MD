using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Tutor.Domain.Entities;

namespace Tutor.Infrastructure.Seeder;

public static class RoleSeeder
{
    public static async Task SeedAsync(ApplicationDbContext context)
    {
        if (!await context.AppRoles.AnyAsync())
        {
            var roles = new List<Role>
            {
                new() { Name = "Admin" }, new() { Name = "Tutor" }, new() { Name = "Student" }
            };

            await context.AppRoles.AddRangeAsync(roles);
            await context.SaveChangesAsync();
            Console.WriteLine("✓ Roles seeded successfully!");
        }
        else
        {
            Console.WriteLine("✓ Roles already exist in database.");
        }
    }
}