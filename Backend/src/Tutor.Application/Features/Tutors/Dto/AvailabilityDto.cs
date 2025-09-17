using System;

namespace Tutor.Application.Features.Tutors.Dto;

public class AvailabilityDto
{
    public int Id { get; set; }
    public int TutorUserId { get; set; }
    public int DayOfWeek { get; set; } // 0=Sun ... 6=Sat
    public TimeOnly StartTime { get; set; }
    public TimeOnly EndTime { get; set; }
    public string Timezone { get; set; }
    public bool ActiveStatus { get; set; }
}