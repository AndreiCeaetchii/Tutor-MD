using Ardalis.Result;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Tutor.Application.Features.Booking.Dto;
using Tutor.Application.Interfaces;
using Tutor.Domain.Entities;
using Tutor.Domain.Interfaces;

namespace Tutor.Application.Services;

public class BookingService : IBookingService
{
    private readonly IGenericRepository<Booking, int> _bookingRepository;
    private readonly IGenericRepository2<User> _userRepository;
    private readonly IGenericRepository<Photo, int> _photoRepository;
    private readonly IGenericRepository<SubjectCatalog, int> _subjectCatalogRepository;
    private readonly IGenericRepository2<TutorSubject> _tutorSubjectRepository;
    private readonly IGenericRepository2<TutorProfile> _tutorProfileRepository;
    private readonly IGenericRepository2<Student> _studentRepository;
    private readonly IMapper _mapper;

    public BookingService(IGenericRepository<Booking, int> bookingRepository,
        IGenericRepository2<User> userRepository,
        IGenericRepository<Photo, int> photoRepository,
        IGenericRepository<SubjectCatalog, int> subjectCatalogRepository,
        IGenericRepository2<TutorSubject> tutorSubjectRepository,
        IGenericRepository2<TutorProfile> tutorProfileRepository,
        IGenericRepository2<Student> studentRepository,
        IMapper mapper)
    {
        _bookingRepository = bookingRepository;
        _userRepository = userRepository;
        _photoRepository = photoRepository;
        _subjectCatalogRepository = subjectCatalogRepository;
        _tutorSubjectRepository = tutorSubjectRepository;
        _tutorProfileRepository = tutorProfileRepository;
        _studentRepository = studentRepository;
        _mapper = mapper;
    }


    public async Task<Result<BookingDto>> CreateBooking(CreateBookingDto createBookingDto, int userId)
    {
        try
        {
            var validationResult = await ValidateBooking(createBookingDto, userId);

            if (!validationResult.IsSuccess)
                return Result<BookingDto>.Error(validationResult.Errors.First());

            var tutorSubject = await _tutorSubjectRepository.FindAsyncDefault(ts =>
                ts.TutorUserId == createBookingDto.TutorUserId &&
                ts.SubjectId == createBookingDto.SubjectId);

            if (tutorSubject == null)
                return Result<BookingDto>.Error("Tutor doesn't teach this subject");

            var hasOverlap = await CheckForOverlappingBookings(createBookingDto, userId);
            if (hasOverlap)
                return Result<BookingDto>.Error("Time slot overlaps with existing booking");

            var booking = _mapper.Map<Booking>(createBookingDto);
            booking.StudentUserId = userId;
            await _bookingRepository.Create(booking);
            var bookingDto = await GetBookingDtoWithDetails(booking);
            return Result<BookingDto>.Success(bookingDto);
        }
        catch (Exception ex)
        {
            return Result<BookingDto>.Error($"Failed to create booking: {ex.Message}");
        }
    }

    private async Task<Result> ValidateBooking(CreateBookingDto createBookingDto, int userId)
    {
        var tutor = await _tutorProfileRepository.FindAsyncDefault(u => u.UserId == createBookingDto.TutorUserId && u.VerificationStatus==VerificationStatus.Verified) ;
        if (tutor == null)
            return Result.Error("Tutor not found");
        var student = await _studentRepository.FindAsyncDefault(u => u.UserId == userId);
        if (student == null)
            return Result.Error("Student not found or Profile did not completed yet");
        var subject = await _subjectCatalogRepository.GetById(createBookingDto.SubjectId);
        if (subject == null)
            return Result.Error("Subject not found");
        if (createBookingDto.StartTime >= createBookingDto.EndTime)
            return Result.Error("Start time must be before end time");
        if (createBookingDto.StartTime < DateTime.UtcNow)
            return Result.Error("Cannot book in the past");
        return Result.Success();
    }

    private async Task<bool> CheckForOverlappingBookings(CreateBookingDto createBookingDto, int userId)
    {
        var existingBookings = await _bookingRepository.FindAsync(b =>
            (b.StudentUserId == userId || b.TutorUserId == createBookingDto.TutorUserId) &&
            b.Status != BookingStatus.Cancelled &&
            ((b.StartTime < createBookingDto.StartTime && b.EndTime > createBookingDto.StartTime) ||
             (b.StartTime < createBookingDto.EndTime && b.EndTime > createBookingDto.EndTime) ||
             (b.StartTime > createBookingDto.StartTime && b.EndTime < createBookingDto.EndTime)));

        return existingBookings.Any();
    }

    private async Task<BookingDto> GetBookingDtoWithDetails(Booking booking)
    {
        var tutor = await _userRepository.FindAsyncDefault(u => u.Id == booking.TutorUserId);
        var tutorPhoto = await _photoRepository.FindAsyncDefault(p => p.Id == tutor.PhotoId);
        var student = await _userRepository.FindAsyncDefault(u => u.Id == booking.StudentUserId);
        var studentPhoto = await _photoRepository.FindAsyncDefault(p => p.Id == student.PhotoId);
        var tutorSubject = await _tutorSubjectRepository.FindAsyncDefault(ts =>
            ts.TutorUserId == booking.TutorUserId &&
            ts.SubjectId == booking.SubjectId);

        var duration = (decimal)(booking.EndTime - booking.StartTime).TotalHours;
        var price = tutorSubject.Price * duration;

        return new BookingDto
        {
            Id = booking.Id,
            TutorUserId = booking.TutorUserId,
            TutorName = $"{tutor?.FirstName} {tutor?.LastName}",
            StudentUserId = booking.StudentUserId,
            StudentName = $"{student?.FirstName} {student?.LastName}",
            Price = price,
            StartTime = booking.StartTime,
            EndTime = booking.EndTime,
            Description = booking.Description,
            Status = booking.Status,
            StudentPhoto = studentPhoto?.Url,
            TutorPhoto = tutorPhoto?.Url
        };
    }

    public async Task<Result<BookingDto>> GetBookingById(int bookingId, int userId)
    {
        var booking = await _bookingRepository.GetById(bookingId);
        if (booking == null)
            return Result<BookingDto>.Error("Booking not found");
        if (booking.StudentUserId != userId && booking.TutorUserId != userId)
            return Result<BookingDto>.Error("Access denied");
        var bookingDto = await GetBookingDtoWithDetails(booking);
        return Result<BookingDto>.Success(bookingDto);
    }

    public async Task<Result<List<BookingDto>>> GetBookingsByUSer(int userId)
    {
        var bookings = await _bookingRepository.FindAsync(b => b.StudentUserId == userId || b.TutorUserId == userId);
        if (bookings.Count == 0)
            return Result<List<BookingDto>>.Error("Bookings not found");
        var bookingDtos = new List<BookingDto>();
        foreach (var booking in bookings)
        {
            var bookingDto = await GetBookingDtoWithDetails(booking);
           bookingDtos.Add(bookingDto);
        }
        return Result<List<BookingDto>>.Success(bookingDtos);
        
    }

    public async Task<Result<BookingDto>> UpdateBookingStatus(int bookingId, int userId, BookingStatus newStatus)
    {
        var booking = await _bookingRepository.GetById(bookingId);
        if (booking == null)
            return Result.Error("Booking not found");

        if (booking.StudentUserId != userId && booking.TutorUserId != userId)
            return Result.Error("Access denied");

        if (!IsValidStatusTransition(booking.Status, newStatus))
            return Result.Error("Invalid status transition");
        
        if(booking.Status == BookingStatus.Confirmed && booking.TutorUserId != userId)
            return Result.Error("Just Tutors can accept booking");
        booking.Status = newStatus;
        await _bookingRepository.Update(booking);
        var bookingDto = await GetBookingDtoWithDetails(booking);
        return Result<BookingDto>.Success(bookingDto);
    }

    private bool IsValidStatusTransition(BookingStatus current, BookingStatus next)
    {
        var validTransitions = new Dictionary<BookingStatus, BookingStatus[]>
        {
            [BookingStatus.Pending] = new[] { BookingStatus.Confirmed, BookingStatus.Cancelled },
            [BookingStatus.Confirmed] = new[] { BookingStatus.Completed, BookingStatus.Cancelled },
            [BookingStatus.Cancelled] = new[] { BookingStatus.Cancelled },
            [BookingStatus.Completed] = new[] { BookingStatus.Completed }
        };

        return validTransitions[current].Contains(next);
    }
}