using System.Collections.Generic;
using System.Threading.Tasks;
using PropertyManagement.Domain.Entities;

namespace PropertyManagement.Application.Interfaces;

public interface IPropertySaleRepository
{
    Task<PropertySale?> GetByIdAsync(int id);
    Task<PropertySale?> GetByIdByOwnerIdAsync(int ownerId, int id);
    Task<List<PropertySale>> GetAllAsync();
    Task<List<PropertySale>> GetAllByOwnerIdAsync(int ownerId);
    Task AddAsync(PropertySale propertySale);
    Task UpdateAsync(PropertySale propertySale);
    Task DeleteAsync(int id);
}
