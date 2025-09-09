using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Tutor.Domain.Entities;
using Tutor.Domain.Entities.Common;

namespace Tutor.Infrastructure.Configuration;

public class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.HasKey(x => x.Id);

            builder.Property(x => x.Username)
                .HasMaxLength(50);

            builder.HasIndex(x => x.Username)
                .IsUnique();

            builder.Property(x => x.Email)
                .IsRequired()
                .HasMaxLength(255);

            builder.HasIndex(x => x.Email)
                .IsUnique();
            

            builder.Property(x => x.Phone)
                .HasMaxLength(20);

            builder.Property(x => x.FirstName)
                .HasMaxLength(100);

            builder.Property(x => x.LastName)
                .HasMaxLength(100);

            builder.Property(x => x.IsActive)
                .IsRequired()
                .HasDefaultValue(true);

            builder.Property(x => x.CreatedAt)
                .HasColumnType("timestamp without time zone")
                .IsRequired();

            builder.Property(x => x.UpdatedAt)
                .HasColumnType("timestamp without time zone")
                .IsRequired();

            // Relationships
            builder.HasOne(x => x.Photo)
                .WithMany(x => x.Users)
                .HasForeignKey(x => x.PhotoId);

            builder.HasOne(x => x.Student)
                .WithOne(x => x.User)
                .HasForeignKey<Student>(x => x.UserId);

            builder.HasOne(x => x.Tutor)
                .WithOne(x => x.User)
                .HasForeignKey<Domain.Entities.Tutor>(x => x.UserId);

            builder.HasMany(x => x.ReceivedNotifications)
                .WithOne(x => x.Recipient)
                .HasForeignKey(x => x.RecipientUserId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasMany(x => x.SentNotifications)
                .WithOne(x => x.Actor)
                .HasForeignKey(x => x.ActorUserId)
                .OnDelete(DeleteBehavior.SetNull);
        }
    }
