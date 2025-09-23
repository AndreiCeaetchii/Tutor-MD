using System;

namespace Tutor.Application.Features.Tutors.Dto;

public class CreateAvailabilityDto
{
    public DateOnly Date{ get; set; } 
    public TimeOnly StartTime { get; set; }
    public TimeOnly EndTime { get; set; }
    public string? Timezone { get; set; }
    public bool ActiveStatus { get; set; } = false;
}