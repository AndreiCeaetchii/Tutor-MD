using Ardalis.Result;
using AutoMapper;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Tutor.Application.Features.Notifications.Dto;
using Tutor.Application.Interfaces;
using Tutor.Domain.Entities;
using Tutor.Domain.Interfaces;

namespace Tutor.Application.Services;

public class NotificationService : INotificationService
{
    private readonly IGenericRepository<Notification, int> _notificationRepository;
    private readonly IMapper _mapper;

    public NotificationService(IGenericRepository<Notification, int> notificationRepository,
        IMapper mapper)
    {
        _notificationRepository = notificationRepository;
        _mapper = mapper;
    }

    public async Task<Result<NotificationCountDto>> GetUnreadNotificationCount(int userId)
    {
        var userNotifications = await _notificationRepository.FindAsync(
            n => n.RecipientUserId == userId && n.Status == NotificationStatus.Unread);

        var count = userNotifications.Count;
        
        return Result.Success(new NotificationCountDto 
        { 
            Count = count
        });
    }

    public async Task<Result<NotificationsDto>> GetUserNotifications(int userId)
    {
        var notifications = await _notificationRepository.FindAsync(
            n => n.RecipientUserId == userId);
    
        notifications = notifications.OrderByDescending(n => n.CreatedAt).ToList();
        var totalCount = notifications.Count;
    
        var notificationDtos = _mapper.Map<List<NotificationDto>>(notifications);
    
        return Result.Success(new NotificationsDto
        {
            TotalCount = totalCount,
            Notifications = notificationDtos
        });
    }

    public async Task<Result> MarkAsRead(int notificationId, int userId)
    {
        var notification = await _notificationRepository.GetById(notificationId);
        
        if (notification == null)
            return Result.NotFound($"Notification with ID {notificationId} not found");
        if (notification.Status == NotificationStatus.Read)
            return Result.Error($"Notification with ID {notificationId} already read");
        if (notification.RecipientUserId != userId)
            return Result.Forbidden("You cannot mark as read others notifications");
        
        notification.Status = NotificationStatus.Read;
        await _notificationRepository.Update(notification);
        
        return Result.Success();
    }
    
    
}