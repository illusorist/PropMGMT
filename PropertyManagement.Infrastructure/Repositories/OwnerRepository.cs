using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using PropertyManagement.Application.Interfaces;
using PropertyManagement.Domain.Entities;
using PropertyManagement.Infrastructure.Data;

namespace PropertyManagement.Infrastructure.Repositories;

public class OwnerRepository : BaseRepository<Owner>, IOwnerRepository
{
    public OwnerRepository(AppDbContext db) : base(db) { }

    public async Task<Owner?> GetByNationalIdAsync(string nationalId)
        => await _db.Owners.FirstOrDefaultAsync(o => o.NationalId == nationalId);

    public async Task<Owner?> GetByUserIdAsync(int userId)
        => await _db.Owners.FirstOrDefaultAsync(o => o.UserId == userId);
}
