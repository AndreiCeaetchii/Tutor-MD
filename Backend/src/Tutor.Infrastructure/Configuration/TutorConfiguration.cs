using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Tutor.Domain.Entities;

namespace Tutor.Infrastructure.Configuration;

public class TutorConfiguration : IEntityTypeConfiguration<Domain.Entities.TutorProfile>
{
    public void Configure(EntityTypeBuilder<Domain.Entities.TutorProfile> builder)
    {
        builder.HasKey(x => x.UserId);

        builder.Property(x => x.VerificationStatus)
            .IsRequired()
            .HasDefaultValue(VerificationStatus.Pending);
        

        // Relationships
        builder.HasOne(x => x.User)
            .WithOne(x => x.TutorProfile)
            .HasForeignKey<Domain.Entities.TutorProfile>(x => x.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(x => x.TutorSubjects)
            .WithOne(x => x.TutorProfile)
            .HasForeignKey(x => x.TutorUserId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(x => x.AvailabilityRules)
            .WithOne(x => x.TutorProfile)
            .HasForeignKey(x => x.TutorUserId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(x => x.Bookings)
            .WithOne(x => x.TutorProfile)
            .HasForeignKey(x => x.TutorUserId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(x => x.Reviews)
            .WithOne(x => x.TutorProfile)
            .HasForeignKey(x => x.TutorUserId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
