using Ardalis.Result;
using System.Collections.Generic;
using System.Threading.Tasks;
using Tutor.Application.Features.Tutors.Dto;

namespace Tutor.Application.Interfaces;

public interface ITutorService
{
     Task<Result<TutorProfileDto>> CreateTutorProfileAsync(CreateTutorProfileDto createTutorProfileDto, int UserId);

      Task<Result<TutorProfileDto>> GetTutorProfileAsync(int userId);
      
      Task<Result<List<TutorProfileDto>>> GetAllTutorProfileAsync();
}