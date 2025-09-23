using Ardalis.Result;
using MediatR;
using System.Threading;
using System.Threading.Tasks;
using Tutor.Application.Features.Notifications.Dto;
using Tutor.Application.Features.Notifications.GetNotifications;
using Tutor.Application.Interfaces;

namespace Tutor.Application.Features.Notifications.ReadNotification;

public class ReadNotificationHandler : IRequestHandler<ReadNotificationCommand, Result<NotificationDto>>
{
    private readonly INotificationService _notificationService;

    public ReadNotificationHandler(INotificationService notificationService)
    {
        _notificationService = notificationService;
    }
    public async Task<Result<NotificationDto>> Handle(ReadNotificationCommand request, CancellationToken cancellationToken)
    {
        return await _notificationService.MarkAsRead( request.NotificationId,request.UserId);
    }
}