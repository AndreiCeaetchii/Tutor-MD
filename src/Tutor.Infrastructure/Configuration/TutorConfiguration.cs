using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Tutor.Domain.Entities;

namespace Tutor.Infrastructure.Configuration;

public class TutorConfiguration : IEntityTypeConfiguration<Domain.Entities.Tutor>
{
    public void Configure(EntityTypeBuilder<Domain.Entities.Tutor> builder)
    {
        builder.HasKey(x => x.UserId);

        builder.Property(x => x.VerificationStatus)
            .IsRequired()
            .HasDefaultValue(VerificationStatus.Pending);

        builder.Property(x => x.CreatedAt)
            .IsRequired()
            .HasDefaultValueSql("GETUTCDATE()");

        builder.Property(x => x.UpdatedAt)
            .IsRequired()
            .HasDefaultValueSql("GETUTCDATE()");

        // Relationships
        builder.HasOne(x => x.User)
            .WithOne(x => x.Tutor)
            .HasForeignKey<Domain.Entities.Tutor>(x => x.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(x => x.TutorSubjects)
            .WithOne(x => x.Tutor)
            .HasForeignKey(x => x.TutorUserId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(x => x.AvailabilityRules)
            .WithOne(x => x.Tutor)
            .HasForeignKey(x => x.TutorUserId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(x => x.Bookings)
            .WithOne(x => x.Tutor)
            .HasForeignKey(x => x.TutorUserId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(x => x.Reviews)
            .WithOne(x => x.Tutor)
            .HasForeignKey(x => x.TutorUserId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
