using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using PropertyManagement.Application.Interfaces;
using PropertyManagement.Domain.Entities;
using PropertyManagement.Infrastructure.Data;

namespace PropertyManagement.Infrastructure.Repositories;

public class PropertySaleRepository : BaseRepository<PropertySale>, IPropertySaleRepository
{
    public PropertySaleRepository(AppDbContext db) : base(db) { }

    public override async Task<PropertySale?> GetByIdAsync(int id)
    {
        return await _db.PropertySales
            .Include(s => s.Property)
            .Include(s => s.BuyerClient)
            .FirstOrDefaultAsync(s => s.Id == id);
    }

    public override async Task<List<PropertySale>> GetAllAsync()
    {
        return await _db.PropertySales
            .Include(s => s.Property)
            .Include(s => s.BuyerClient)
            .ToListAsync();
    }

    public async Task<PropertySale?> GetByIdByOwnerIdAsync(int ownerId, int id)
    {
        return await _db.PropertySales
            .Include(s => s.Property)
            .Include(s => s.BuyerClient)
            .FirstOrDefaultAsync(s => s.Property.OwnerId == ownerId && s.Id == id);
    }

    public async Task<List<PropertySale>> GetAllByOwnerIdAsync(int ownerId)
    {
        return await _db.PropertySales
            .Include(s => s.Property)
            .Include(s => s.BuyerClient)
            .Where(s => s.Property.OwnerId == ownerId)
            .ToListAsync();
    }
}
