using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;
using Tutor.Application.Interfaces;
using Tutor.Domain.Entities;

namespace Tutor.Infrastructure.Seeder;

public static class AdminSeeder
{
    public static async Task SeedAsync(ApplicationDbContext context, IPasswordHasher passwordHasher)
    {
        var adminEmail = "admina@test.com";
        var existingAdmin = await context.Users.FirstOrDefaultAsync(u => u.Email == adminEmail);

        if (existingAdmin == null)
        {
            // 1. Create the admin user
            var adminUser = new User
            {
                Email = adminEmail,
                Username = "admin",
                FirstName = "System",
                LastName = "Administrator",
                IsActive = true,
                LastLoginAt = DateTime.UtcNow
            };

            await context.AppUsers.AddAsync(adminUser);
            await context.SaveChangesAsync(); // Save so admin gets an Id

            // 2. Hash the password
            var passwordHash = passwordHasher.HashPassword("Admin1.");

            var password = new Password { UserId = adminUser.Id, PasswordHash = passwordHash };
            await context.Passwords.AddAsync(password);

            // 3. Assign Admin role (RoleId = 1)
            var userRole = new UserRole { UserId = adminUser.Id, RoleId = 1, AssignedAt = DateTime.UtcNow };
            await context.UserRoles.AddAsync(userRole);

            await context.SaveChangesAsync();

            Console.WriteLine("✓ Admin user seeded successfully!");
        }
        else
        {
            Console.WriteLine("✓ Admin already exists in database.");
        }
    }
}