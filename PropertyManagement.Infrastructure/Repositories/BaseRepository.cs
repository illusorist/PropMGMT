using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using PropertyManagement.Domain.Entities;
using PropertyManagement.Infrastructure.Data;

namespace PropertyManagement.Infrastructure.Repositories;

public class BaseRepository<T> where T : BaseEntity
{
    protected readonly AppDbContext _db;
    public BaseRepository(AppDbContext db) => _db = db;

    public virtual async Task<T?> GetByIdAsync(int id) => await _db.Set<T>().FindAsync(id);
    public virtual async Task<List<T>> GetAllAsync() => await _db.Set<T>().ToListAsync();
    public virtual async Task AddAsync(T entity)
    {
        await _db.Set<T>().AddAsync(entity);
        await _db.SaveChangesAsync();
    }

    public virtual async Task UpdateAsync(T entity)
    {
        _db.Set<T>().Update(entity);
        await _db.SaveChangesAsync();
    }

    public virtual async Task DeleteAsync(int id)
    {
        var entity = await GetByIdAsync(id);
        if (entity != null)
        {
            _db.Set<T>().Remove(entity);
            await _db.SaveChangesAsync();
        }
    }
}
