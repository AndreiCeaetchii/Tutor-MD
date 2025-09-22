using AutoMapper;
using System;
using Tutor.Application.Features.Booking.Dto;
using Tutor.Domain.Entities;

namespace Tutor.Application.Mappers;

    public class BookingMappingProfile : Profile
    {
        public BookingMappingProfile()
        {
            // CreateBookingDto -> Booking (for creation)
            CreateMap<CreateBookingDto, Booking>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.StudentUserId, opt => opt.Ignore()) // Set manually
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => BookingStatus.Pending))
                .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(src => DateTime.UtcNow));


        }
    }