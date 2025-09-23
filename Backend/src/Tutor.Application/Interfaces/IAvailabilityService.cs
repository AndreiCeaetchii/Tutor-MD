using Ardalis.Result;
using System.Collections.Generic;
using System.Threading.Tasks;
using Tutor.Application.Features.Tutors.Dto;

namespace Tutor.Application.Interfaces;

public interface IAvailabilityService
{
    Task<Result<AvailabilityDto>> CreateAvailability(int userId, CreateAvailabilityDto createAvailabilityDto);
    Task<Result<AvailabilityDto>> UpdateAvailability(int userId, AvailabilityDto availabilityDto);
    Task<Result> DeleteAvailability(int userId, int id);
    Task<Result<List<AvailabilityDto>>> GetAvailabilitiesByTutor(int tutorUserId);



}