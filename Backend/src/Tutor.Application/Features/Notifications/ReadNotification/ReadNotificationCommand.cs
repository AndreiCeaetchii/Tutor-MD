using Ardalis.Result;
using MediatR;
using Tutor.Application.Features.Notifications.Dto;

namespace Tutor.Application.Features.Notifications.ReadNotification;

public record ReadNotificationCommand(int UserId, int NotificationId) : IRequest<Result<NotificationDto>>;
