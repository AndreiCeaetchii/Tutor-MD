using Ardalis.Result;
using System.Threading.Tasks;
using Tutor.Application.Features.Notifications.Dto;

namespace Tutor.Application.Interfaces;

public interface INotificationService
{
    Task<Result<NotificationCountDto>> GetUnreadNotificationCount(int userId);
    Task<Result<NotificationsDto>> GetUserNotifications(int userId);
    Task<Result> MarkAsRead(int notificationId, int userId);
    
}