using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using PropertyManagement.Application.Interfaces;
using PropertyManagement.Domain.Entities;
using PropertyManagement.Infrastructure.Data;

namespace PropertyManagement.Infrastructure.Repositories;

public class PaymentRepository : BaseRepository<Payment>, IPaymentRepository
{
    public PaymentRepository(AppDbContext db) : base(db) { }

    public async Task<Payment?> GetByIdByOwnerIdAsync(int ownerId, int id)
    {
        return await _db.Payments
            .Where(p => p.Contract.Property.OwnerId == ownerId && p.Id == id)
            .FirstOrDefaultAsync();
    }

    public async Task<List<Payment>> GetAllByOwnerIdAsync(int ownerId)
    {
        return await _db.Payments
            .Where(p => p.Contract.Property.OwnerId == ownerId)
            .ToListAsync();
    }
}
