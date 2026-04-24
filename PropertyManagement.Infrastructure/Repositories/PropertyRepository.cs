using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using PropertyManagement.Application.Interfaces;
using PropertyManagement.Domain.Entities;
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

    public async Task<List<Property>> GetAllWithAmenitiesAsync()
    {
        return await _db.Properties
            .Include(p => p.PropertyAmenities)
            .ThenInclude(pa => pa.Amenity)
            .ToListAsync();
    }

    public override async Task UpdateAsync(Property property)
    {
        await _db.SaveChangesAsync();
    }
}
