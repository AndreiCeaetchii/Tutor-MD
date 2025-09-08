using System;
using System.Collections.Generic;
using Tutor.Domain.Entities.Common;

namespace Tutor.Domain.Entities;

public class User : Entity<int>
{
    public string? Username { get; set; }
    public string Email { get; set; }
    public string? Phone { get; set; }
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public string? Bio { get; set; }
    public DateTime? Birthdate { get; set; }
    public int? PhotoId { get; set; }
    public bool IsActive { get; set; }
    public DateTime LastLoginAt { get; set; }
    
    // Navigation properties
    public virtual Photo Photo { get; set; }
    public virtual ICollection<UserRole> UserRoles { get; set; } = new List<UserRole>();
    public virtual Student Student { get; set; }
    public virtual Tutor Tutor { get; set; }
    public virtual ICollection<Notification> ReceivedNotifications { get; set; } = new List<Notification>();
    public virtual ICollection<Notification> SentNotifications { get; set; } = new List<Notification>();
    
    public virtual Password Password    { get; set; }
    
    public virtual GoogleAuth GoogleAuth { get; set; }
}