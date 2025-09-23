using System.Collections.Generic;

namespace Tutor.Application.Features.Notifications.Dto;

public class NotificationsDto
{
    public required int TotalCount { get; set; }
    
    public required List<NotificationDto> Notifications { get; set; }
    
   
}