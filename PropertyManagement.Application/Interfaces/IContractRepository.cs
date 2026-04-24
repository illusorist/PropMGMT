using System.Collections.Generic;
using System.Threading.Tasks;
using PropertyManagement.Domain.Entities;

namespace PropertyManagement.Application.Interfaces;

public interface IContractRepository
{
    Task<Contract?> GetByIdAsync(int id);
    Task<List<Contract>> GetAllAsync();
    Task AddAsync(Contract contract);
    Task UpdateAsync(Contract contract);
    Task DeleteAsync(int id);
}
