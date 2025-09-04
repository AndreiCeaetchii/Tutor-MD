using NodaTime;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Tutor.Domain.Entities.Common;
public abstract class Entity<T>
{
    public Entity()
    {
        CreatedAt = UpdatedAt = DateTime.Now; // or DateTime.UtcNow
    }
    
    public virtual T Id { get; set; } = default!;
    
    [Column(TypeName = "timestamp without time zone")]
    public DateTime CreatedAt { get; set; }
    
    [Column(TypeName = "timestamp without time zone")]
    public DateTime UpdatedAt { get; set; }
    
}