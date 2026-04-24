using System.Collections.Generic;
using System.Threading.Tasks;
using PropertyManagement.Domain.Entities;

namespace PropertyManagement.Application.Interfaces;

public interface IUnitRepository
{
    Task<Unit?> GetByIdAsync(int id);
    Task<List<Unit>> GetAllAsync();
    Task AddAsync(Unit unit);
    Task UpdateAsync(Unit unit);
    Task DeleteAsync(int id);
}
