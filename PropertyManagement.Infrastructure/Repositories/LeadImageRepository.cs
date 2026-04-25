using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using PropertyManagement.Application.Interfaces;
using PropertyManagement.Domain.Entities;
using PropertyManagement.Infrastructure.Data;

namespace PropertyManagement.Infrastructure.Repositories;

public class LeadImageRepository : BaseRepository<LeadImage>, ILeadImageRepository
{
    public LeadImageRepository(AppDbContext db) : base(db) { }

    public async Task<LeadImage?> GetByIdForLeadAsync(int leadId, int id)
    {
        return await _db.LeadImages.FirstOrDefaultAsync(i => i.LeadId == leadId && i.Id == id);
    }

    public async Task<List<LeadImage>> GetByLeadIdAsync(int leadId)
    {
        return await _db.LeadImages
            .Where(i => i.LeadId == leadId)
            .OrderBy(i => i.SortOrder)
            .ThenBy(i => i.Id)
            .ToListAsync();
    }

    public async Task<int> GetMaxSortOrderAsync(int leadId)
    {
        var max = await _db.LeadImages
            .Where(i => i.LeadId == leadId)
            .Select(i => (int?)i.SortOrder)
            .MaxAsync();
        return max ?? 0;
    }
}
