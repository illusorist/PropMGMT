using System.Collections.Generic;
using System.Threading.Tasks;
using PropertyManagement.Domain.Entities;

namespace PropertyManagement.Application.Interfaces;

public interface IBuyerClientRepository
{
    Task<BuyerClient?> GetByIdAsync(int id);
    Task<List<BuyerClient>> GetAllAsync();
    Task AddAsync(BuyerClient buyerClient);
    Task UpdateAsync(BuyerClient buyerClient);
    Task DeleteAsync(int id);
}
