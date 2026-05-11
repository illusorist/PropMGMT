using System.Collections.Generic;
using System.Threading.Tasks;
using PropertyManagement.Domain.Entities;

namespace PropertyManagement.Application.Interfaces;

public interface ICommercialListingRepository
{
    Task<CommercialListing?> GetByIdAsync(int id);
    Task AddAsync(CommercialListing listing);
    Task UpdateAsync(CommercialListing listing);
    Task DeleteAsync(int id);
    Task<(List<CommercialListing> Items, int Total)> SearchAsync(
        string? q,
        string? status,
        string? employee,
        string? broker,
        int page,
        int pageSize,
        string? sortBy,
        string? sortDir);
}
