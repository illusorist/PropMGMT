using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using PropertyManagement.Application.Interfaces;
using PropertyManagement.Domain.Entities;
using PropertyManagement.Infrastructure.Data;

namespace PropertyManagement.Infrastructure.Repositories;

public class ContractRepository : BaseRepository<Contract>, IContractRepository
{
    public ContractRepository(AppDbContext db) : base(db) { }

    public async Task<Contract?> GetByIdByOwnerIdAsync(int ownerId, int id)
    {
        return await _db.Contracts
            .Where(c => c.Property.OwnerId == ownerId && c.Id == id)
            .FirstOrDefaultAsync();
    }

    public async Task<List<Contract>> GetAllByOwnerIdAsync(int ownerId)
    {
        return await _db.Contracts
            .Where(c => c.Property.OwnerId == ownerId)
            .ToListAsync();
    }
}
