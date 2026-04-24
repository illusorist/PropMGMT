using System.Collections.Generic;
using System.Threading.Tasks;
using PropertyManagement.Domain.Entities;

namespace PropertyManagement.Application.Interfaces;

public interface IOwnerRepository
{
    Task<Owner?> GetByIdAsync(int id);
    Task<Owner?> GetByUserIdAsync(int userId);
    Task<List<Owner>> GetAllAsync();
    Task AddAsync(Owner owner);
    Task UpdateAsync(Owner owner);
    Task DeleteAsync(int id);
}
