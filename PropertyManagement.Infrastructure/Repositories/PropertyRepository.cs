using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using PropertyManagement.Application.Interfaces;
using PropertyManagement.Domain.Entities;
using PropertyManagement.Domain.Enums;
using PropertyManagement.Infrastructure.Data;

namespace PropertyManagement.Infrastructure.Repositories;

public class PropertyRepository : BaseRepository<Property>, IPropertyRepository
{
    public PropertyRepository(AppDbContext db) : base(db) { }

    public async Task<Property?> GetByIdWithAmenitiesAsync(int id)
    {
        return await _db.Properties
            .Include(p => p.PropertyAmenities)
            .ThenInclude(pa => pa.Amenity)
            .FirstOrDefaultAsync(p => p.Id == id);
    }

    public async Task<Property?> GetByIdWithAmenitiesByOwnerIdAsync(int ownerId, int id)
    {
        return await _db.Properties
            .Include(p => p.PropertyAmenities)
            .ThenInclude(pa => pa.Amenity)
            .FirstOrDefaultAsync(p => p.OwnerId == ownerId && p.Id == id);
    }

    public async Task<List<Property>> GetAllWithAmenitiesAsync()
    {
        return await _db.Properties
            .Include(p => p.PropertyAmenities)
            .ThenInclude(pa => pa.Amenity)
            .ToListAsync();
    }

    public async Task<List<Property>> GetAllWithAmenitiesByOwnerIdAsync(int ownerId)
    {
        return await _db.Properties
            .Include(p => p.PropertyAmenities)
            .ThenInclude(pa => pa.Amenity)
            .Where(p => p.OwnerId == ownerId)
            .ToListAsync();
    }

    public async Task<int> CountByOwnerAsync(int ownerId)
        => await _db.Properties.CountAsync(p => p.OwnerId == ownerId);

    public async Task<int> CountByOwnerAndStatusAsync(int ownerId, PropertyStatus status)
        => await _db.Properties.CountAsync(p => p.OwnerId == ownerId && p.Status == status);

    public override async Task UpdateAsync(Property property)
    {
        await _db.SaveChangesAsync();
    }
}
