using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Tutor.Domain.Entities;

namespace Tutor.Infrastructure.Configuration;

public class StudentConfiguration : IEntityTypeConfiguration<Student>
{
    public void Configure(EntityTypeBuilder<Student> builder)
    {
        builder.HasKey(x => x.UserId);
        

        // Relationships
        builder.HasOne(x => x.User)
            .WithOne(x => x.Student)
            .HasForeignKey<Student>(x => x.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(x => x.Bookings)
            .WithOne(x => x.Student)
            .HasForeignKey(x => x.StudentUserId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(x => x.Reviews)
            .WithOne(x => x.Student)
            .HasForeignKey(x => x.StudentUserId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
