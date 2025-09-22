using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Tutor.Domain.Entities;

namespace Tutor.Infrastructure.Configuration;

public class GoogleAuthConfiguration : IEntityTypeConfiguration<GoogleAuth>
{
    public void Configure(EntityTypeBuilder<GoogleAuth> builder)
    {
        builder.ToTable("google_auth");

        builder.HasKey(g => g.Id);

        builder.Property(g => g.OAuthProvider)
            .IsRequired();

        builder.Property(g => g.OAuthProviderId)
            .IsRequired();

        builder.HasOne(g => g.User)
            .WithOne(u => u.GoogleAuth)
            .HasForeignKey<GoogleAuth>(g => g.UserId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}