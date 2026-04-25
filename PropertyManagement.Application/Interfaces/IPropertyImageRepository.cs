using System.Collections.Generic;
using System.Threading.Tasks;
using PropertyManagement.Domain.Entities;

namespace PropertyManagement.Application.Interfaces;

public interface IPropertyImageRepository
{
    Task<PropertyImage?> GetByIdAsync(int id);
    Task<PropertyImage?> GetByIdForPropertyAsync(int propertyId, int id);
    Task<List<PropertyImage>> GetByPropertyIdAsync(int propertyId);
    Task<int> GetMaxSortOrderAsync(int propertyId);
    Task AddAsync(PropertyImage image);
    Task UpdateAsync(PropertyImage image);
    Task UpdateRangeAsync(List<PropertyImage> images);
    Task ClearPrimaryAsync(int propertyId);
    Task DeleteAsync(int id);
}
