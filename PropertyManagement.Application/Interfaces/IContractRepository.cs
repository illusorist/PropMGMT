using System.Collections.Generic;
using System.Threading.Tasks;
using PropertyManagement.Domain.Entities;
using PropertyManagement.Domain.Enums;

namespace PropertyManagement.Application.Interfaces;

public interface IContractRepository
{
    Task<Contract?> GetByIdAsync(int id);
    Task<Contract?> GetByIdByOwnerIdAsync(int ownerId, int id);
    Task<List<Contract>> GetAllAsync();
    Task<List<Contract>> GetAllByOwnerIdAsync(int ownerId);
    Task<int> CountByOwnerAsync(int ownerId);
    Task<int> CountByOwnerAndStatusAsync(int ownerId, ContractStatus status);
    Task AddAsync(Contract contract);
    Task UpdateAsync(Contract contract);
    Task DeleteAsync(int id);
}
