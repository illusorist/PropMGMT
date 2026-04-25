using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using PropertyManagement.Application.Interfaces;
using PropertyManagement.Domain.Entities;
using PropertyManagement.Infrastructure.Data;

namespace PropertyManagement.Infrastructure.Repositories;

public class PropertyImageRepository : BaseRepository<PropertyImage>, IPropertyImageRepository
{
    public PropertyImageRepository(AppDbContext db) : base(db) { }

    public async Task<PropertyImage?> GetByIdForPropertyAsync(int propertyId, int id)
    {
        return await _db.PropertyImages.FirstOrDefaultAsync(i => i.PropertyId == propertyId && i.Id == id);
    }

    public async Task<List<PropertyImage>> GetByPropertyIdAsync(int propertyId)
    {
        return await _db.PropertyImages
            .Where(i => i.PropertyId == propertyId)
            .OrderByDescending(i => i.IsPrimary)
            .ThenBy(i => i.SortOrder)
            .ThenBy(i => i.Id)
            .ToListAsync();
    }

    public async Task<int> GetMaxSortOrderAsync(int propertyId)
    {
        var max = await _db.PropertyImages
            .Where(i => i.PropertyId == propertyId)
            .Select(i => (int?)i.SortOrder)
            .MaxAsync();
        return max ?? 0;
    }

    public async Task UpdateRangeAsync(List<PropertyImage> images)
    {
        _db.PropertyImages.UpdateRange(images);
        await _db.SaveChangesAsync();
    }

    public async Task ClearPrimaryAsync(int propertyId)
    {
        var primaries = await _db.PropertyImages
            .Where(i => i.PropertyId == propertyId && i.IsPrimary)
            .ToListAsync();
        if (primaries.Count == 0)
            return;

        foreach (var image in primaries)
            image.IsPrimary = false;

        await _db.SaveChangesAsync();
    }
}
