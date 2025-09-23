using Tutor.Domain.Entities;

namespace Tutor.Application.Features.Notifications.Dto;

public class NotificationDto
{
    public required int Id { get; set; }
    public required string Type { get; set; }
    public required string Payload { get; set; } 
    public NotificationStatus Status { get; set; }
}