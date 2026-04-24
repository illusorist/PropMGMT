using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PropertyManagement.Application.DTOs.Contract;
using PropertyManagement.Application.Interfaces;
using PropertyManagement.Domain.Entities;

namespace PropertyManagement.Application.Services;

public class ContractService
{
    private readonly IContractRepository _repo;
    public ContractService(IContractRepository repo) => _repo = repo;

    public async Task<List<ContractResponseDto>> GetAllAsync()
    {
        var contracts = await _repo.GetAllAsync();
        return contracts.Select(c => new ContractResponseDto
        {
            Id = c.Id,
            PropertyId = c.PropertyId,
            TenantId = c.TenantId,
            StartDate = c.StartDate,
            EndDate = c.EndDate,
            MonthlyRent = c.MonthlyRent,
            Status = c.Status,
            CreatedAt = c.CreatedAt
        }).ToList();
    }

    public async Task<ContractResponseDto?> GetByIdAsync(int id)
    {
        var c = await _repo.GetByIdAsync(id);
        if (c == null) return null;
        return new ContractResponseDto
        {
            Id = c.Id,
            PropertyId = c.PropertyId,
            TenantId = c.TenantId,
            StartDate = c.StartDate,
            EndDate = c.EndDate,
            MonthlyRent = c.MonthlyRent,
            Status = c.Status,
            CreatedAt = c.CreatedAt
        };
    }

    public async Task<List<ContractResponseDto>> GetAllForOwnerAsync(int ownerId)
    {
        var contracts = await _repo.GetAllByOwnerIdAsync(ownerId);
        return contracts.Select(c => new ContractResponseDto
        {
            Id = c.Id,
            PropertyId = c.PropertyId,
            TenantId = c.TenantId,
            StartDate = c.StartDate,
            EndDate = c.EndDate,
            MonthlyRent = c.MonthlyRent,
            Status = c.Status,
            CreatedAt = c.CreatedAt
        }).ToList();
    }

    public async Task<ContractResponseDto?> GetByIdForOwnerAsync(int ownerId, int id)
    {
        var c = await _repo.GetByIdByOwnerIdAsync(ownerId, id);
        if (c == null) return null;
        return new ContractResponseDto
        {
            Id = c.Id,
            PropertyId = c.PropertyId,
            TenantId = c.TenantId,
            StartDate = c.StartDate,
            EndDate = c.EndDate,
            MonthlyRent = c.MonthlyRent,
            Status = c.Status,
            CreatedAt = c.CreatedAt
        };
    }

    public async Task CreateAsync(ContractCreateDto dto)
    {
        var contract = new Contract
        {
            PropertyId = dto.PropertyId,
            TenantId = dto.TenantId,
            StartDate = dto.StartDate,
            EndDate = dto.EndDate,
            MonthlyRent = dto.MonthlyRent,
            Status = dto.Status
        };
        await _repo.AddAsync(contract);
    }

    public async Task UpdateAsync(int id, ContractCreateDto dto)
    {
        var contract = await _repo.GetByIdAsync(id)
            ?? throw new KeyNotFoundException($"Contract {id} not found");
        contract.PropertyId = dto.PropertyId;
        contract.TenantId = dto.TenantId;
        contract.StartDate = dto.StartDate;
        contract.EndDate = dto.EndDate;
        contract.MonthlyRent = dto.MonthlyRent;
        contract.Status = dto.Status;
        contract.UpdatedAt = DateTime.UtcNow;
        await _repo.UpdateAsync(contract);
    }

    public async Task DeleteAsync(int id) => await _repo.DeleteAsync(id);
}
