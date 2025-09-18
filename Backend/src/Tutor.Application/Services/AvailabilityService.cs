using Ardalis.Result;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Tutor.Application.Features.Tutors.Dto;
using Tutor.Application.Interfaces;
using Tutor.Domain.Entities;
using Tutor.Domain.Interfaces;

namespace Tutor.Application.Services;

public class AvailabilityService : IAvailabilityService
{
    private readonly IGenericRepository<TutorAvailabilityRule, int> _availabilityRepository;
    private readonly IGenericRepository2<TutorProfile> _tutorRepository;
    private readonly IMapper _mapper;

    public AvailabilityService(IGenericRepository<TutorAvailabilityRule, int> availabilityRepository,
        IGenericRepository2<TutorProfile> tutorRepository,
        IMapper mapper)
    {
        _availabilityRepository = availabilityRepository;
        _tutorRepository = tutorRepository;
        _mapper = mapper;
    }

    public async Task<Result<AvailabilityDto>> CreateAvailability(int userId,
        CreateAvailabilityDto createAvailabilityDto)
    {
        try
        {
            if (createAvailabilityDto.StartTime >= createAvailabilityDto.EndTime)
                return Result<AvailabilityDto>.Error("Start time must be before end time");

            if (createAvailabilityDto.DayOfWeek < 0 || createAvailabilityDto.DayOfWeek > 6)
                return Result<AvailabilityDto>.Error("Day of week must be between 0 and 6");

            var availabilityRule = _mapper.Map<TutorAvailabilityRule>(createAvailabilityDto);
            availabilityRule.TutorUserId = userId;

            await _availabilityRepository.Create(availabilityRule);

            var resultDto = _mapper.Map<AvailabilityDto>(availabilityRule);

            return Result<AvailabilityDto>.Success(resultDto);
        }
        catch (Exception ex)
        {
            return Result<AvailabilityDto>.Error($"Failed to create availability: {ex.Message}");
        }
    }

    public async Task<Result<AvailabilityDto>> UpdateAvailability(int userId,
        AvailabilityDto availabilityDto)
    {
        try
        {
            if (availabilityDto.StartTime >= availabilityDto.EndTime)
                return Result<AvailabilityDto>.Error("Start time must be before end time");

            if (availabilityDto.DayOfWeek < 0 || availabilityDto.DayOfWeek > 6)
                return Result<AvailabilityDto>.Error("Day of week must be between 0 and 6");

            var availabilityRule = await _availabilityRepository.GetById(availabilityDto.Id);
            
            if (availabilityRule == null)
                return Result<AvailabilityDto>.Error("AvailabilityRule not found");
            if(availabilityRule.TutorUserId != userId)
                return Result<AvailabilityDto>.Error("You don't have permission to update availability");
            
            availabilityRule = _mapper.Map(availabilityDto, availabilityRule);
            
            await _availabilityRepository.Update(availabilityRule);

            var resultDto = _mapper.Map<AvailabilityDto>(availabilityRule);

            return Result<AvailabilityDto>.Success(resultDto);
        }
        catch (Exception ex)
        {
            return Result<AvailabilityDto>.Error($"Failed to update availability: {ex.Message}");
        }
    }

    public async Task<Result> DeleteAvailability(int userId, int id)
    {
        try
        {
            var availabilityRule = await _availabilityRepository.GetById(id);
            
            if (availabilityRule == null)
                return Result.Error("AvailabilityRule not found");
            if(availabilityRule.TutorUserId != userId)
                return Result.Error("You don't have permission to update availability");
            
            await _availabilityRepository.Delete(availabilityRule);

            return Result.Success();
        }
        catch (Exception ex)
        {
            return Result.Error($"Failed to update availability: {ex.Message}");
        }
    }


    public async Task<Result<AvailabilityDto>> GetAvailabilityById(int id)
    {
        var availability = await _availabilityRepository.GetById(id);
        if (availability == null)
            return Result<AvailabilityDto>.Error("Availability not found");

        return Result<AvailabilityDto>.Success(_mapper.Map<AvailabilityDto>(availability));
    }

    public async Task<Result<List<AvailabilityDto>>> GetAvailabilitiesByTutor(int tutorUserId)
    {
        var tutorAvailabilities = await _availabilityRepository.FindAsync(u => u.TutorUserId == tutorUserId);
        if (tutorAvailabilities.Count == 0)
            return Result<List<AvailabilityDto>>.Error("Tutor does not have any availability");
        return Result<List<AvailabilityDto>>.Success(
            _mapper.Map<List<AvailabilityDto>>(tutorAvailabilities)
        );
    }
}