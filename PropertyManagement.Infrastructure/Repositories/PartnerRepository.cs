using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using PropertyManagement.Application.Interfaces;
using PropertyManagement.Domain.Entities;
using PropertyManagement.Infrastructure.Data;

namespace PropertyManagement.Infrastructure.Repositories;

public class PartnerRepository : IPartnerRepository
{
    private readonly AppDbContext _db;

    public PartnerRepository(AppDbContext db) => _db = db;

    public async Task<List<Partner>> GetAllAsync()
        => await _db.Partners.ToListAsync();

    public async Task<Partner?> GetByIdAsync(Guid id)
        => await _db.Partners.FirstOrDefaultAsync(p => p.Id == id);

    public async Task<Partner?> GetByUserIdAsync(int userId)
        => await _db.Partners.FirstOrDefaultAsync(p => p.UserId == userId);

    public async Task AddAsync(Partner partner)
    {
        await _db.Partners.AddAsync(partner);
        await _db.SaveChangesAsync();
    }

    public async Task UpdateAsync(Partner partner)
    {
        _db.Partners.Update(partner);
        await _db.SaveChangesAsync();
    }

    public async Task DeleteAsync(Guid id)
    {
        var partner = await GetByIdAsync(id);
        if (partner == null) return;
        _db.Partners.Remove(partner);
        await _db.SaveChangesAsync();
    }
}
