using System.Collections.Generic;
using System.Threading.Tasks;
using PropertyManagement.Domain.Entities;

namespace PropertyManagement.Application.Interfaces;

public interface IAmenityRepository
{
    Task<Amenity?> GetByIdAsync(int id);
    Task<List<Amenity>> GetAllAsync();
    Task<List<Amenity>> GetByIdsAsync(List<int> ids);
    Task AddAsync(Amenity amenity);
    Task UpdateAsync(Amenity amenity);
    Task DeleteAsync(int id);
}
