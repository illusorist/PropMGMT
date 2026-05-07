using System.Collections.Generic;
using System.Threading.Tasks;
using PropertyManagement.Domain.Entities;

namespace PropertyManagement.Application.Interfaces;

public interface IRequestRecordRepository
{
    Task<RequestRecord?> GetByIdAsync(int id);
    Task<List<RequestRecord>> GetAllAsync();
    Task AddAsync(RequestRecord request);
    Task<(List<RequestRecord> Items, int Total)> SearchAsync(
        string? q,
        string? status,
        string? employee,
        string? requestType,
        DateTime? fromDate,
        DateTime? toDate,
        int page,
        int pageSize,
        string? sortBy,
        string? sortDir);
}
