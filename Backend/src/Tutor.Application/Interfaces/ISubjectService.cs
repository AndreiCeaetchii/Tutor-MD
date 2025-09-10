using Ardalis.Result;
using System.Threading.Tasks;
using Tutor.Domain.Entities;

namespace Tutor.Application.Interfaces;

public interface ISubjectService
{
    Task<Result<int>> GetOrCreateSubjectIdAsync(string subjectName, string? subjectSlug = null);
    Task<SubjectCatalog?> GetSubjectByIdAsync(int subjectId);
}
