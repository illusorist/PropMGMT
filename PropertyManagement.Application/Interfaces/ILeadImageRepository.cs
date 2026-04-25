using System.Collections.Generic;
using System.Threading.Tasks;
using PropertyManagement.Domain.Entities;

namespace PropertyManagement.Application.Interfaces;

public interface ILeadImageRepository
{
    Task<LeadImage?> GetByIdAsync(int id);
    Task<LeadImage?> GetByIdForLeadAsync(int leadId, int id);
    Task<List<LeadImage>> GetByLeadIdAsync(int leadId);
    Task<int> GetMaxSortOrderAsync(int leadId);
    Task AddAsync(LeadImage image);
    Task UpdateAsync(LeadImage image);
    Task DeleteAsync(int id);
}
