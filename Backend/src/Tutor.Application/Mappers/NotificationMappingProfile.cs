using AutoMapper;
using System;
using Tutor.Application.Features.Booking.Dto;
using Tutor.Application.Features.Notifications.Dto;
using Tutor.Domain.Entities;

namespace Tutor.Application.Mappers;

public class NotificationMappingProfile : Profile
{
    public NotificationMappingProfile()
    {
        CreateMap<Notification, NotificationDto>();


    }
}