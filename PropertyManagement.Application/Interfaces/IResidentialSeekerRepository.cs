using System.Collections.Generic;
using System.Threading.Tasks;
using PropertyManagement.Domain.Entities;

namespace PropertyManagement.Application.Interfaces;

public interface IResidentialSeekerRepository
{
    Task<ResidentialSeeker?> GetByIdAsync(int id);
    Task AddAsync(ResidentialSeeker seeker);
    Task UpdateAsync(ResidentialSeeker seeker);
    Task DeleteAsync(int id);
    Task<(List<ResidentialSeeker> Items, int Total)> SearchAsync(
        string? q,
        string? status,
        string? employee,
        int page,
        int pageSize,
        string? sortBy,
        string? sortDir);
}
