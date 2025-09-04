using Tutor.Domain.Entities.Common;

namespace Tutor.Domain.Entities;

public class Notification : Entity<int>
{
    public int RecipientUserId { get; set; }
    public int? ActorUserId { get; set; }
    public string Type { get; set; }
    public string Payload { get; set; } // JSON
    public NotificationStatus Status { get; set; }

    // Navigation properties
    public virtual User Recipient { get; set; }
    public virtual User Actor { get; set; }
}

public enum NotificationStatus
{
    Unread,
    Read
}