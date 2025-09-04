using System;
using Tutor.Domain.Entities.Common;

namespace Tutor.Domain.Entities;

public class TutorAvailabilityRule : Entity<int>
{
    public int TutorUserId { get; set; }
    public int DayOfWeek { get; set; } // 0=Sun ... 6=Sat
    public TimeOnly StartTime { get; set; }
    public TimeOnly EndTime { get; set; }
    public string Timezone { get; set; }
    public bool ActiveStatus { get; set; }

    // Navigation properties
    public virtual Tutor Tutor { get; set; }
}