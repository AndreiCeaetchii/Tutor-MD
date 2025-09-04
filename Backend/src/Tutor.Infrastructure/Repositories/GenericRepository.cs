using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Tutor.Domain.Entities.Common;
using Tutor.Domain.Interfaces;

namespace Tutor.Infrastructure.Repositories;


public class GenericRepository<T, TId> : IGenericRepository<T, TId>
    where T : Entity<TId>
{
    protected readonly ApplicationDbContext _context;
    public DbSet<T> Table => _context.Set<T>();

    public GenericRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<T>> GetAll()
    {
        return await Table.ToListAsync();
    }

    public async Task<T?> GetById(TId id)
    {
        return await Table.FindAsync(id);
    }

    public async Task Create(T entity)
    {
        await Table.AddAsync(entity);
        await _context.SaveChangesAsync();
    }

    public async Task Update(T entity)
    {
        Table.Update(entity);
        await _context.SaveChangesAsync();
    }

    public async Task Delete(T entity)
    {
        Table.Remove(entity);
        await _context.SaveChangesAsync();
    }

    public async Task<List<T>> FindAsync(Expression<Func<T, bool>> predicate)
    {
        return await Table.Where(predicate).ToListAsync();
    }

    public async Task<T?> FindAsyncDefault(Expression<Func<T, bool>> predicate)
    {
        return await Table.FirstOrDefaultAsync(predicate);
    }
}