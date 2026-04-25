using System.Collections.Generic;
using System.Threading.Tasks;
using PropertyManagement.Domain.Entities;
using PropertyManagement.Domain.Enums;

namespace PropertyManagement.Application.Interfaces;

public interface ILeadRepository
{
    Task<Lead?> GetByIdAsync(int id);
    Task<Lead?> GetByIdWithDetailsAsync(int id);
    Task<List<Lead>> GetAllWithDetailsAsync(LeadIntent? intent, LeadStatus? status);
    Task AddAsync(Lead lead);
    Task UpdateAsync(Lead lead);
    Task DeleteAsync(int id);
}