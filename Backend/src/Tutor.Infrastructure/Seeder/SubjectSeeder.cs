using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Tutor.Domain.Entities;

namespace Tutor.Infrastructure.Seeder;

public static class SubjectSeeder
{
    public static async Task SeedAsync(ApplicationDbContext context)
    {
        if (!await context.SubjectCatalog.AnyAsync())
        {
            var subjects = new List<SubjectCatalog>
            {
                new() { Name = "Mathematics", Slug = "mathematics" },
                new() { Name = "Physics", Slug = "physics" },
                new() { Name = "Chemistry", Slug = "chemistry" },
                new() { Name = "Biology", Slug = "biology" },
                new() { Name = "English", Slug = "english" },
                new() { Name = "History", Slug = "history" },
                new() { Name = "Computer Science", Slug = "computer-science" },
                new() { Name = "Spanish", Slug = "spanish" },
                new() { Name = "Art", Slug = "art" },
                new() { Name = "Music", Slug = "music" }
            };

            await context.SubjectCatalog.AddRangeAsync(subjects);
            await context.SaveChangesAsync();
            Console.WriteLine("✓ Subjects seeded successfully!");
        }
        else
        {
            Console.WriteLine("✓ Subjects already exist in database.");
        }
    }
}