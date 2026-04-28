using System.Collections.Generic;
using System.Threading.Tasks;
using PropertyManagement.Domain.Entities;

namespace PropertyManagement.Application.Interfaces;

public interface IPaymentRepository
{
    Task<Payment?> GetByIdAsync(int id);
    Task<Payment?> GetByIdByOwnerIdAsync(int ownerId, int id);
    Task<List<Payment>> GetAllAsync();
    Task<List<Payment>> GetAllByOwnerIdAsync(int ownerId);
    Task<List<Payment>> GetAllByContractIdAsync(int contractId);
    Task AddAsync(Payment payment);
    Task UpdateAsync(Payment payment);
    Task DeleteAsync(int id);
}
