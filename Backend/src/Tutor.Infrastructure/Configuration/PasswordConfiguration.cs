using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Tutor.Domain.Entities;

namespace Tutor.Infrastructure.Configuration;

public class PasswordConfiguration : IEntityTypeConfiguration<Password>
{
    public void Configure(EntityTypeBuilder<Password> builder)
    {
        builder.ToTable("passwords");

        builder.HasKey(p => p.Id);

        builder.Property(p => p.PasswordHash)
            .IsRequired()
            .HasMaxLength(255);

        builder.HasOne(p => p.User)
            .WithOne(u => u.Password)
            .HasForeignKey<Password>(p => p.UserId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}