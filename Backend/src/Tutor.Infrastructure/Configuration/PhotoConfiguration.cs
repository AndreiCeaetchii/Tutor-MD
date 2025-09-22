using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Tutor.Domain.Entities;

namespace Tutor.Infrastructure.Configuration;

public class PhotoConfiguration : IEntityTypeConfiguration<Photo>
{
    public void Configure(EntityTypeBuilder<Photo> builder)
    {
        builder.HasKey(x => x.Id);
            
        builder.Property(x => x.PublicId)
            .HasMaxLength(191);

        builder.Property(x => x.Url)
            .HasMaxLength(2048);

        builder.Property(x => x.Provider)
            .HasMaxLength(50);

        builder.Property(x => x.MimeType)
            .HasMaxLength(100);

        builder.Property(x => x.CreatedAt)
            .HasColumnType("timestamp without time zone")
            .IsRequired();

        // Relationships
        builder.HasMany(x => x.Users)
            .WithOne(x => x.Photo)
            .HasForeignKey(x => x.PhotoId)
            .OnDelete(DeleteBehavior.SetNull);
    }
}