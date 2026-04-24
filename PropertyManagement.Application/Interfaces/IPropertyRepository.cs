using System.Collections.Generic;
using System.Threading.Tasks;
using PropertyManagement.Domain.Entities;

namespace PropertyManagement.Application.Interfaces;

public interface IPropertyRepository
{
    Task<Property?> GetByIdAsync(int id);
    Task<List<Property>> GetAllAsync();
    Task AddAsync(Property property);
    Task UpdateAsync(Property property);
    Task DeleteAsync(int id);
}
