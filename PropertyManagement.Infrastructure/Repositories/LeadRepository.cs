using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using PropertyManagement.Application.Interfaces;
using PropertyManagement.Domain.Entities;
using PropertyManagement.Domain.Enums;
using PropertyManagement.Infrastructure.Data;

namespace PropertyManagement.Infrastructure.Repositories;

public class LeadRepository : BaseRepository<Lead>, ILeadRepository
{
    public LeadRepository(AppDbContext db) : base(db) { }

    public async Task<Lead?> GetByIdWithDetailsAsync(int id)
    {
        return await _db.Leads
            .Include(l => l.Property)
            .Include(l => l.AssignedToUser)
            .Include(l => l.Images)
            .FirstOrDefaultAsync(l => l.Id == id);
    }

    public async Task<Lead?> GetByIdWithDetailsAndImagesAsync(int id)
    {
        return await _db.Leads
            .Include(l => l.Property)
            .Include(l => l.AssignedToUser)
            .Include(l => l.Images)
            .FirstOrDefaultAsync(l => l.Id == id);
    }

    public async Task<List<Lead>> GetAllWithDetailsAsync(LeadIntent? intent, LeadStatus? status)
    {
        var query = _db.Leads
            .Include(l => l.Property)
            .Include(l => l.AssignedToUser)
            .Include(l => l.Images)
            .AsQueryable();

        if (intent.HasValue)
            query = query.Where(l => l.Intent == intent.Value);

        if (status.HasValue)
            query = query.Where(l => l.Status == status.Value);

        return await query
            .OrderByDescending(l => l.CreatedAt)
            .ToListAsync();
    }
}