using System;

namespace Tutor.Domain.Entities;

public class UserRole
{
    public int UserId { get; set; }
    public int RoleId { get; set; }
    public DateTime AssignedAt { get; set; }

    // Navigation properties
    public virtual User User { get; set; }
    public virtual Role Role { get; set; }
}