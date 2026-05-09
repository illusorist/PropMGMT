using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using PropertyManagement.Application.Interfaces;
using PropertyManagement.Domain.Entities;
using PropertyManagement.Infrastructure.Data;

namespace PropertyManagement.Infrastructure.Repositories;

public class ResidentialSeekerRepository : BaseRepository<ResidentialSeeker>, IResidentialSeekerRepository
{
    public ResidentialSeekerRepository(AppDbContext db) : base(db) { }

    public async Task<(List<ResidentialSeeker> Items, int Total)> SearchAsync(
        string? q,
        string? status,
        string? employee,
        int page,
        int pageSize,
        string? sortBy,
        string? sortDir)
    {
        var normalizedPage = page <= 0 ? 1 : page;
        var normalizedPageSize = pageSize <= 0 ? 20 : Math.Min(pageSize, 100);

        var query = _db.ResidentialSeekers.AsNoTracking().AsQueryable();

        if (!string.IsNullOrWhiteSpace(q))
        {
            var like = $"%{q.Trim()}%";
            query = query.Where(r =>
                EF.Functions.ILike(r.FullName, like)
                || EF.Functions.ILike(r.Mobile, like)
                || EF.Functions.ILike(r.PreferredLocation, like));
        }

        if (!string.IsNullOrWhiteSpace(status))
        {
            var like = $"%{status.Trim()}%";
            query = query.Where(r => EF.Functions.ILike(r.Status, like));
        }

        if (!string.IsNullOrWhiteSpace(employee))
        {
            var like = $"%{employee.Trim()}%";
            query = query.Where(r => EF.Functions.ILike(r.Employee, like));
        }

        var sortKey = (sortBy ?? string.Empty).Trim().ToLowerInvariant().Replace("_", string.Empty);
        var isDesc = !string.Equals(sortDir, "asc", StringComparison.OrdinalIgnoreCase);
        query = sortKey switch
        {
            "requestdate" => isDesc ? query.OrderByDescending(r => r.RequestDate) : query.OrderBy(r => r.RequestDate),
            "fullname" => isDesc ? query.OrderByDescending(r => r.FullName) : query.OrderBy(r => r.FullName),
            "status" => isDesc ? query.OrderByDescending(r => r.Status) : query.OrderBy(r => r.Status),
            "employee" => isDesc ? query.OrderByDescending(r => r.Employee) : query.OrderBy(r => r.Employee),
            _ => query.OrderByDescending(r => r.CreatedAt)
        };

        var total = await query.CountAsync();
        var items = await query
            .Skip((normalizedPage - 1) * normalizedPageSize)
            .Take(normalizedPageSize)
            .ToListAsync();

        return (items, total);
    }
}
