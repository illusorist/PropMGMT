using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using PropertyManagement.Domain.Entities;

namespace PropertyManagement.Application.Interfaces;

public interface IPartnerRepository
{
    Task<List<Partner>> GetAllAsync();
    Task<Partner?> GetByIdAsync(Guid id);
    Task<Partner?> GetByUserIdAsync(int userId);
    Task AddAsync(Partner partner);
    Task UpdateAsync(Partner partner);
    Task DeleteAsync(Guid id);
}
