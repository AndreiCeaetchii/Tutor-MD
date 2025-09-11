using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Tutor.Domain.Entities;
using Tutor.Domain.Interfaces;

namespace Tutor.Infrastructure.Repositories;

public class GenericRepository2<T> : IGenericRepository2<T> where T : class
{
    private readonly ApplicationDbContext _context;
    public DbSet<T> Table { get; }

    public GenericRepository2(ApplicationDbContext context)
    {
        _context = context;
        Table = _context.Set<T>();
    }

    public async Task<IEnumerable<T>> GetAll()
    {
        return await Table.ToListAsync();
    }

    public async Task<T?> GetById(int? id)
    {
        return await Table.FindAsync(id);
    }

    public async Task Create(T entity)
    {
        await Table.AddAsync(entity);
        await SaveChangesAsync();
    }

    public async Task Update(T entity)
    {
        await SaveChangesAsync();
    }

    public async Task Delete(T entity)
    {
        Table.Remove(entity);
        await SaveChangesAsync();
    }

    public async Task<T?> FindAsyncDefault(Expression<Func<T, bool>> predicate)
    {
        return await Table.Where(predicate).FirstOrDefaultAsync();
    }

    public async Task<List<T>> FindAsync(Expression<Func<T, bool>> predicate)
    {
        return await Table.Where(predicate).ToListAsync();
    }

    private async Task SaveChangesAsync()
    {
        await _context.SaveChangesAsync();
    }
    
}