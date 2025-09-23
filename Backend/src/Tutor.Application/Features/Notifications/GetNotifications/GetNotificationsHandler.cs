using Ardalis.Result;
using MediatR;
using System.Threading;
using System.Threading.Tasks;
using Tutor.Application.Features.Notifications.Dto;
using Tutor.Application.Interfaces;

namespace Tutor.Application.Features.Notifications.GetNotifications;

public class GetNotificationsHandler : IRequestHandler<GetNotificationsCommand, Result<NotificationsDto>>
{private readonly INotificationService _notificationService;

    public GetNotificationsHandler(INotificationService notificationService)
    {
        _notificationService = notificationService;
    }
    
    public async Task<Result<NotificationsDto>> Handle(GetNotificationsCommand request, CancellationToken cancellationToken)
    {
        return await _notificationService.GetUserNotifications(request.UserId);
    }
}