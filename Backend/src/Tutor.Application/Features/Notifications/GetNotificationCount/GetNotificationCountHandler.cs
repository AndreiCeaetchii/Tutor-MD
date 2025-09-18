using Ardalis.Result;
using MediatR;
using System.Threading;
using System.Threading.Tasks;
using Tutor.Application.Features.Notifications.Dto;
using Tutor.Application.Interfaces;

namespace Tutor.Application.Features.Notifications.GetNotificationCount;

public class GetNotificationCountHandler : IRequestHandler<GetNotificationCountCommand,Result<NotificationCountDto>>
{
    private readonly INotificationService _notificationService;

    public GetNotificationCountHandler(INotificationService notificationService)
    {
        _notificationService = notificationService;
    }
    public async Task<Result<NotificationCountDto>> Handle(GetNotificationCountCommand request, CancellationToken cancellationToken)
    {
        return await _notificationService.GetUnreadNotificationCount(request.UserId);
    }
}