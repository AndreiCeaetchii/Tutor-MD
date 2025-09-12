using Ardalis.Result;
using AutoMapper;
using System.Linq;
using System.Threading.Tasks;
using Tutor.Application.Features.Students.DTOs;
using Tutor.Application.Interfaces;
using Tutor.Domain.Entities;
using Tutor.Domain.Interfaces;

namespace Tutor.Application.Services;

public class StudentService : IStudentService
{
    private readonly IGenericRepository2<Student> _studentRepository;
    private readonly IGenericRepository<User, int> _userRepository;
    private readonly IUserRoleService _userRoleService;
    private readonly IMapper _mapper;

    public StudentService(IGenericRepository2<Student> studentRepository, IGenericRepository<User, int> userRepository,
       IUserRoleService userRoleService, IMapper mapper)
    {
        _userRepository = userRepository;
        _studentRepository = studentRepository;
        _userRoleService = userRoleService;
        _mapper = mapper;
    }
    public async Task<Result<StudentDto>> GetStudentAsync(int userId)
    {
        var profile = await _studentRepository.FindAsyncDefault(tp => tp.UserId == userId);
        if (profile is null)
            return Result<StudentDto>.NotFound("Tutor profile not found");

        return _mapper.Map<StudentDto>(profile);
    }
    
    public async Task<Result<StudentDto>> CreateStudentAsync(CreateStudentDto createStudentDto,
        int userId)
    {
        var user = await _userRepository.GetById(userId);
        if (user is null)
            return Result<StudentDto>.NotFound("User not found");

        var existingProfile = await _studentRepository.FindAsyncDefault(tp => tp.UserId == userId);
        if (existingProfile is not null)
            return Result<StudentDto>.Error("Tutor profile already exists");

        if (await _userRoleService.HasAnyRoleAsync(userId))
            return Result<StudentDto>.Error("User already has a role assigned");

        var studentProfile = new Student
        {
            UserId = userId,
            Grade = createStudentDto.Grade,
            Class = createStudentDto.Class,
        };

        await _studentRepository.Create(studentProfile);

        var roleResult = await _userRoleService.AssignStudentRoleAsync(userId);
        if (!roleResult.IsSuccess)
            return Result<StudentDto>.Error(roleResult.Errors.FirstOrDefault());
        return await GetStudentAsync(userId);

    }
   
    
    
}