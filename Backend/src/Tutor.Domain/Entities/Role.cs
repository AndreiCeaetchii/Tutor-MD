using System.Collections.Generic;
using Tutor.Domain.Entities.Common;

namespace Tutor.Domain.Entities;

public class Role : Entity<int>
{
    public string Name { get; set; } // 'student', 'tutor', 'admin'

    // Navigation properties
    public virtual ICollection<UserRole> UserRoles { get; set; } = new List<UserRole>();
}