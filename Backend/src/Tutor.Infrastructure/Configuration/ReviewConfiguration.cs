using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Tutor.Domain.Entities;

namespace Tutor.Infrastructure.Configuration;

public class ReviewConfiguration : IEntityTypeConfiguration<Review>
{
    public void Configure(EntityTypeBuilder<Review> builder)
    {
        builder.HasKey(x => x.Id);

        builder.Property(x => x.Rating)
            .IsRequired();

        builder.Property(x => x.CreatedAt)
            .HasColumnType("timestamp without time zone")
            .IsRequired();

        builder.Property(x => x.UpdatedAt)
            .HasColumnType("timestamp without time zone")
            .IsRequired();

        // Relationships
        builder.HasOne(x => x.Booking)
            .WithOne(x => x.Review)
            .HasForeignKey<Review>(x => x.BookingId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(x => x.Tutor)
            .WithMany(x => x.Reviews)
            .HasForeignKey(x => x.TutorUserId)
            .OnDelete(DeleteBehavior.NoAction);

        builder.HasOne(x => x.Student)
            .WithMany(x => x.Reviews)
            .HasForeignKey(x => x.StudentUserId)
            .OnDelete(DeleteBehavior.NoAction);
    }
}