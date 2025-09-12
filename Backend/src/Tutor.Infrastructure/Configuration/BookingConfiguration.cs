using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Tutor.Domain.Entities;

namespace Tutor.Infrastructure.Configuration;

public class BookingConfiguration : IEntityTypeConfiguration<Booking>
{
    public void Configure(EntityTypeBuilder<Booking> builder)
    {
        builder.HasKey(x => x.Id);

        builder.Property(x => x.StartTime)
            .IsRequired()
            .HasColumnType("timestamp with time zone");

        builder.Property(x => x.EndTime)
            .IsRequired()
            .HasColumnType("timestamp with time zone");

        builder.Property(x => x.PriceSnapshot)
            .IsRequired()
            .HasColumnType("decimal(8,2)");

        builder.Property(x => x.Status)
            .IsRequired()
            .HasDefaultValue(BookingStatus.Pending);

        builder.Property(x => x.CreatedAt)
            .HasColumnType("timestamp without time zone")
            .IsRequired();

        builder.Property(x => x.UpdatedAt)
            .HasColumnType("timestamp without time zone")
            .IsRequired();

        // Relationships
        builder.HasOne(x => x.TutorProfile)
            .WithMany(x => x.Bookings)
            .HasForeignKey(x => x.TutorUserId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(x => x.Student)
            .WithMany(x => x.Bookings)
            .HasForeignKey(x => x.StudentUserId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(x => x.Subject)
            .WithMany(x => x.Bookings)
            .HasForeignKey(x => x.SubjectId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(x => x.Review)
            .WithOne(x => x.Booking)
            .HasForeignKey<Review>(x => x.BookingId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}