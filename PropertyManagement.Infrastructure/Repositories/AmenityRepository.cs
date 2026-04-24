using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using PropertyManagement.Application.Interfaces;
using PropertyManagement.Domain.Entities;
using PropertyManagement.Infrastructure.Data;

namespace PropertyManagement.Infrastructure.Repositories;

public class AmenityRepository : BaseRepository<Amenity>, IAmenityRepository
{
    public AmenityRepository(AppDbContext db) : base(db) { }

    public async Task<List<Amenity>> GetByIdsAsync(List<int> ids)
    {
        return await _db.Amenities
            .Where(a => ids.Contains(a.Id))
            .ToListAsync();
    }
}
