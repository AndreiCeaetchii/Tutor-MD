using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Tutor.Domain.Entities.Common;

namespace Tutor.Domain.Interfaces;

public interface IGenericRepository<T, TId> where T : Entity<TId>
{
    DbSet<T> Table { get; }

    Task<IEnumerable<T>> GetAll();
    Task<T?> GetById(TId id);
    Task Create(T entity);
    Task Update(T entity);
    Task Delete(T entity);
    Task<List<T>> FindAsync(Expression<Func<T, bool>> predicate);
    Task<T?> FindAsyncDefault(Expression<Func<T, bool>> predicate);
}