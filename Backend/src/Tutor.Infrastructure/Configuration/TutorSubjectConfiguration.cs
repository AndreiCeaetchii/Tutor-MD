using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Tutor.Domain.Entities;

namespace Tutor.Infrastructure.Configuration;

public class TutorSubjectConfiguration : IEntityTypeConfiguration<TutorSubject>
{
    public void Configure(EntityTypeBuilder<TutorSubject> builder)
    {
        builder.HasKey(x => new { x.TutorUserId, x.SubjectId });

        builder.Property(x => x.Price)
            .IsRequired()
            .HasColumnType("decimal(8,2)");

        builder.Property(x => x.Currency)
            .IsRequired()
            .HasMaxLength(3);

        // Relationships
        builder.HasOne(x => x.TutorProfile)
            .WithMany(x => x.TutorSubjects)
            .HasForeignKey(x => x.TutorUserId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(x => x.Subject)
            .WithMany(x => x.TutorSubjects)
            .HasForeignKey(x => x.SubjectId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}