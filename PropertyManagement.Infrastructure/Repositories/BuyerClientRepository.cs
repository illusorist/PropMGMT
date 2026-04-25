using PropertyManagement.Application.Interfaces;
using PropertyManagement.Domain.Entities;
using PropertyManagement.Infrastructure.Data;

namespace PropertyManagement.Infrastructure.Repositories;

public class BuyerClientRepository : BaseRepository<BuyerClient>, IBuyerClientRepository
{
    public BuyerClientRepository(AppDbContext db) : base(db) { }
}
