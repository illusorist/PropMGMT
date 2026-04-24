using PropertyManagement.Application.Interfaces;
using PropertyManagement.Domain.Entities;
using PropertyManagement.Infrastructure.Data;

namespace PropertyManagement.Infrastructure.Repositories;

public class ContractRepository : BaseRepository<Contract>, IContractRepository
{
    public ContractRepository(AppDbContext db) : base(db) { }
}
