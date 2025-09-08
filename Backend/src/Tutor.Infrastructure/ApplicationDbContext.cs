using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Text.RegularExpressions;
using Tutor.Application.Common;
using Tutor.Domain.Entities;
using Tutor.Infrastructure.Configuration;

namespace Tutor.Infrastructure;

    public class ApplicationDbContext
        : IdentityDbContext<ApplicationUser, ApplicationRole, Guid>, IContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        // Existing entities
        public DbSet<Hero> Heroes { get; set; } = null!;
        public DbSet<User> AppUsers { get; set; } = null!;
        public DbSet<Photo> Photos { get; set; } = null!;
        public DbSet<Role> AppRoles { get; set; } = null!;
        public DbSet<UserRole> UserRoles { get; set; } = null!;
        public DbSet<Student> Students { get; set; } = null!;
        public DbSet<Domain.Entities.Tutor> Tutors { get; set; } = null!;
        public DbSet<SubjectCatalog> SubjectCatalog { get; set; } = null!;
        public DbSet<TutorSubject> TutorSubjects { get; set; } = null!;
        public DbSet<TutorAvailabilityRule> TutorAvailabilityRules { get; set; } = null!;
        public DbSet<Booking> Bookings { get; set; } = null!;
        public DbSet<Review> Reviews { get; set; } = null!;
        public DbSet<Notification> Notifications { get; set; } = null!;
        public DbSet<Password> Passwords { get; set; } = null!;
        public DbSet<GoogleAuth> GoogleAuths { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            // Apply existing configurations
            builder.ApplyConfigurationsFromAssembly(typeof(HeroConfiguration).Assembly);

            builder.ApplyConfiguration(new PhotoConfiguration());
            builder.ApplyConfiguration(new UserConfiguration());
            builder.ApplyConfiguration(new RoleConfiguration());
            builder.ApplyConfiguration(new UserRoleConfiguration());
            builder.ApplyConfiguration(new StudentConfiguration());
            builder.ApplyConfiguration(new TutorConfiguration());
            builder.ApplyConfiguration(new SubjectCatalogConfiguration());
            builder.ApplyConfiguration(new TutorSubjectConfiguration());
            builder.ApplyConfiguration(new TutorAvailabilityRuleConfiguration());
            builder.ApplyConfiguration(new BookingConfiguration());
            builder.ApplyConfiguration(new ReviewConfiguration());
            builder.ApplyConfiguration(new NotificationConfiguration());
            
            
        }
    }

        
    
