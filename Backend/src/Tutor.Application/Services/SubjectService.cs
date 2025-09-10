using Ardalis.Result;
using System;
using System.Threading.Tasks;
using Tutor.Application.Interfaces;
using Tutor.Domain.Entities;
using Tutor.Domain.Interfaces;

namespace Tutor.Application.Services;

public class SubjectService : ISubjectService
{
    private readonly IGenericRepository<SubjectCatalog, int> _subjectRepository;

    public SubjectService(IGenericRepository<SubjectCatalog, int> subjectRepository)
    {
        _subjectRepository = subjectRepository;
    }

    public async Task<Result<int>> GetOrCreateSubjectIdAsync(string subjectName, string? subjectSlug = null)
    {
        var slug = string.IsNullOrEmpty(subjectSlug)
            ? GenerateSlug(subjectName)
            : subjectSlug;

        var existingSubject = await _subjectRepository.FindAsyncDefault(s =>
            s.Name.ToLower() == subjectName.ToLower() ||
            s.Slug.ToLower() == slug.ToLower());

        if (existingSubject is not null)
            return Result<int>.Success(existingSubject.Id);

        var newSubject = new SubjectCatalog { Name = subjectName, Slug = slug };

        await _subjectRepository.Create(newSubject);
        return Result<int>.Success(newSubject.Id);
    }

    public async Task<SubjectCatalog?> GetSubjectByIdAsync(int subjectId) =>
        await _subjectRepository.GetById(subjectId);


    private string GenerateSlug(string name) =>
        name.ToLower().Replace(" ", "-").Replace("--", "-").Trim('-');
}