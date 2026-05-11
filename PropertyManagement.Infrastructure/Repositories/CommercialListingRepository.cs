using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using PropertyManagement.Application.Interfaces;
using PropertyManagement.Domain.Entities;
using PropertyManagement.Infrastructure.Data;

namespace PropertyManagement.Infrastructure.Repositories;

public class CommercialListingRepository : BaseRepository<CommercialListing>, ICommercialListingRepository
{
    public CommercialListingRepository(AppDbContext db) : base(db) { }

    public async Task<(List<CommercialListing> Items, int Total)> SearchAsync(
        string? q,
        string? status,
        string? employee,
        string? broker,
        int page,
        int pageSize,
        string? sortBy,
        string? sortDir)
    {
        var normalizedPage = page <= 0 ? 1 : page;
        var normalizedPageSize = pageSize <= 0 ? 20 : Math.Min(pageSize, 100);

        var query = _db.CommercialListings.AsNoTracking().AsQueryable();

        if (!string.IsNullOrWhiteSpace(q))
        {
            var like = $"%{q.Trim()}%";
            query = query.Where(r =>
                EF.Functions.ILike(r.OwnerName, like)
                || EF.Functions.ILike(r.Location, like)
                || EF.Functions.ILike(r.PropertyType, like));
        }

        if (!string.IsNullOrWhiteSpace(status))
        {
            var like = $"%{status.Trim()}%";
            query = query.Where(r => EF.Functions.ILike(r.PropertyStatus, like));
        }

        if (!string.IsNullOrWhiteSpace(employee))
        {
            var like = $"%{employee.Trim()}%";
            query = query.Where(r => EF.Functions.ILike(r.Employee, like));
        }

        if (!string.IsNullOrWhiteSpace(broker))
        {
            var brokerName = broker.Trim();
            query = query.Where(r => r.Broker != null && EF.Functions.ILike(r.Broker!, brokerName));
        }

        var sortKey = (sortBy ?? string.Empty).Trim().ToLowerInvariant().Replace("_", string.Empty);
        var isDesc = !string.Equals(sortDir, "asc", StringComparison.OrdinalIgnoreCase);
        query = sortKey switch
        {
            "contactdate" => isDesc ? query.OrderByDescending(r => r.ContactDate) : query.OrderBy(r => r.ContactDate),
            "ownername" => isDesc ? query.OrderByDescending(r => r.OwnerName) : query.OrderBy(r => r.OwnerName),
            "propertystatus" => isDesc ? query.OrderByDescending(r => r.PropertyStatus) : query.OrderBy(r => r.PropertyStatus),
            "propertytype" => isDesc ? query.OrderByDescending(r => r.PropertyType) : query.OrderBy(r => r.PropertyType),
            "rentamount" => isDesc ? query.OrderByDescending(r => r.RentAmount) : query.OrderBy(r => r.RentAmount),
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
