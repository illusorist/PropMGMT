using System.Collections.Generic;
using System.Threading.Tasks;
using PropertyManagement.Domain.Entities;
using PropertyManagement.Domain.Enums;

namespace PropertyManagement.Application.Interfaces;

public interface IPropertyRepository
{
    Task<Property?> GetByIdAsync(int id);
    Task<Property?> GetByIdWithAmenitiesAsync(int id);
    Task<Property?> GetByIdWithAmenitiesByOwnerIdAsync(int ownerId, int id);
    Task<List<Property>> GetAllAsync();
    Task<List<Property>> GetAllWithAmenitiesAsync();
    Task<List<Property>> GetAllWithAmenitiesByOwnerIdAsync(int ownerId);
    Task<int> CountByOwnerAsync(int ownerId);
    Task<int> CountByOwnerAndStatusAsync(int ownerId, PropertyStatus status);
    Task AddAsync(Property property);
    Task UpdateAsync(Property property);
    Task DeleteAsync(int id);
}
