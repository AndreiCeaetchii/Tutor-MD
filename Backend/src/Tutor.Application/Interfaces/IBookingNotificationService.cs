using System.Threading.Tasks;
using Tutor.Domain.Entities;

namespace Tutor.Application.Interfaces;

public interface IBookingNotificationService
{
    Task CreateBookingChangeNotification(Booking booking);

    Task CheckUpcomingBookings();
}