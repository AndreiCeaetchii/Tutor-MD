using System.Collections.Generic;
using Tutor.Domain.Entities.Common;

namespace Tutor.Domain.Entities;

public class Photo : Entity<int>
{
    public string PublicId { get; set; }
    public string Url { get; set; }
    public string Provider { get; set; }
    public int? Width { get; set; }
    public int? Height { get; set; }
    public string MimeType { get; set; }
    public int? Bytes { get; set; }

    // Navigation properties
    public virtual ICollection<User> Users { get; set; } = new List<User>();
}