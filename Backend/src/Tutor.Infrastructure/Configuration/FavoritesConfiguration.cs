using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Tutor.Domain.Entities;

namespace Tutor.Infrastructure.Configuration;

public class FavoritesConfiguration : IEntityTypeConfiguration<Favorites>
{
    public void Configure(EntityTypeBuilder<Favorites> builder)
    {
        // Table name
        builder.ToTable("Favorites");

        // Primary Key
        builder.HasKey(f => f.Id);

        builder.Property(f => f.TutorUserId)
            .IsRequired();

        builder.Property(f => f.StudentUserId)
            .IsRequired();

        // Relationships
        // Many-to-One with TutorProfile
        builder.HasOne(f => f.TutorProfile)
            .WithMany() // or .WithMany(t => t.Favorites) if TutorProfile has a collection
            .HasForeignKey(f => f.TutorUserId)
            .OnDelete(DeleteBehavior.Restrict); // or Cascade based on your needs

        // Many-to-One with Student
        builder.HasOne(f => f.Student)
            .WithMany() // or .WithMany(s => s.Favorites) if Student has a collection
            .HasForeignKey(f => f.StudentUserId)
            .OnDelete(DeleteBehavior.Restrict);

        // Unique constraint to prevent duplicate favorites
        builder.HasIndex(f => new { f.TutorUserId, f.StudentUserId })
            .IsUnique();
    }
}