using System;

namespace Tutor.Domain.Entities.Common;

public abstract class Entity<T>
{
    public Entity()
    {
        CreatedAt = UpdatedAt = DateTime.UtcNow;
    }
    public virtual T Id { get; set; } = default!;
    
    public DateTime CreatedAt { get; set; }
    
    public DateTime UpdatedAt { get; set; }
}