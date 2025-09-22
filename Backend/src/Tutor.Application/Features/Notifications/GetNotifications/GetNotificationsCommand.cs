using Ardalis.Result;
using MediatR;
using Tutor.Application.Features.Notifications.Dto;

namespace Tutor.Application.Features.Notifications.GetNotifications;

public record GetNotificationsCommand(int UserId) : IRequest<Result<NotificationsDto>>;
