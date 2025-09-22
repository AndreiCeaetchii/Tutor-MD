using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Tutor.Domain.Entities;

namespace Tutor.Infrastructure.Configuration;

public class SubjectCatalogConfiguration : IEntityTypeConfiguration<SubjectCatalog>
{
    public void Configure(EntityTypeBuilder<SubjectCatalog> builder)
    {
        builder.HasKey(x => x.Id);

        builder.Property(x => x.Name)
            .IsRequired()
            .HasMaxLength(100);

        builder.HasIndex(x => x.Name)
            .IsUnique();

        builder.Property(x => x.Slug)
            .HasMaxLength(120);

        builder.Property(x => x.CreatedAt)
            .HasColumnType("timestamp without time zone")
            .IsRequired();

        builder.Property(x => x.UpdatedAt)
            .HasColumnType("timestamp without time zone")
            .IsRequired();

        // Relationships
        builder.HasMany(x => x.TutorSubjects)
            .WithOne(x => x.Subject)
            .HasForeignKey(x => x.SubjectId)
            .OnDelete(DeleteBehavior.Cascade);

 
        
    }
}