using Ardalis.Result;
using MediatR;
using Tutor.Application.Features.Notifications.Dto;

namespace Tutor.Application.Features.Notifications.GetNotificationCount;

public record GetNotificationCountCommand(int UserId) : IRequest<Result<NotificationCountDto>>;
