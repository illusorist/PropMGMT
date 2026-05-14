using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PropertyManagement.Application.DTOs.Contract;
using PropertyManagement.Application.Interfaces;
using PropertyManagement.Domain.Entities;
using PropertyManagement.Domain.Enums;

namespace PropertyManagement.Application.Services;

public class ContractService
{
    private readonly IContractRepository _repo;

    public ContractService(IContractRepository repo)
    {
        _repo = repo;
    }

    private static void ValidateContractInput(ContractCreateDto dto)
    {
        if (dto.PropertyId <= 0) throw new ArgumentException("Property is required.");
        if (dto.TenantId <= 0) throw new ArgumentException("Tenant is required.");
        if (string.IsNullOrWhiteSpace(dto.DeedNumber)) throw new ArgumentException("Deed number is required.");
        if (dto.MonthlyRent <= 0) throw new ArgumentException("Monthly rent must be greater than zero.");
        if (dto.StartDate > dto.EndDate) throw new ArgumentException("Start date must be on or before end date.");
    }

    public async Task<List<ContractResponseDto>> GetAllAsync()
    {
        var contracts = await _repo.GetAllAsync();
        return contracts.Select(c => new ContractResponseDto
        {
            Id = c.Id,
            PropertyId = c.PropertyId,
            TenantId = c.TenantId,
            DeedNumber = c.DeedNumber,
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
            DeedNumber = c.DeedNumber,
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
            DeedNumber = c.DeedNumber,
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
            DeedNumber = c.DeedNumber,
            StartDate = c.StartDate,
            EndDate = c.EndDate,
            MonthlyRent = c.MonthlyRent,
            Status = c.Status,
            CreatedAt = c.CreatedAt
        };
    }

    public async Task<ContractResponseDto> CreateAsync(ContractCreateDto dto)
    {
        ValidateContractInput(dto);

        var contract = new Contract
        {
            PropertyId = dto.PropertyId,
            TenantId = dto.TenantId,
            DeedNumber = dto.DeedNumber,
            StartDate = DateTime.SpecifyKind(dto.StartDate, DateTimeKind.Utc),
            EndDate = DateTime.SpecifyKind(dto.EndDate, DateTimeKind.Utc),
            MonthlyRent = dto.MonthlyRent,
            Status = dto.Status
        };
        await _repo.AddAsync(contract);

        return new ContractResponseDto
        {
            Id = contract.Id,
            PropertyId = contract.PropertyId,
            TenantId = contract.TenantId,
            DeedNumber = contract.DeedNumber,
            StartDate = contract.StartDate,
            EndDate = contract.EndDate,
            MonthlyRent = contract.MonthlyRent,
            Status = contract.Status,
            CreatedAt = contract.CreatedAt
        };
    }

    public async Task UpdateAsync(int id, ContractCreateDto dto)
    {
        ValidateContractInput(dto);

        var contract = await _repo.GetByIdAsync(id)
            ?? throw new KeyNotFoundException($"Contract {id} not found");

        contract.PropertyId = dto.PropertyId;
        contract.TenantId = dto.TenantId;
        contract.DeedNumber = dto.DeedNumber;
        contract.StartDate = DateTime.SpecifyKind(dto.StartDate, DateTimeKind.Utc);
        contract.EndDate = DateTime.SpecifyKind(dto.EndDate, DateTimeKind.Utc);
        contract.MonthlyRent = dto.MonthlyRent;
        contract.Status = dto.Status;
        contract.UpdatedAt = DateTime.UtcNow;
        await _repo.UpdateAsync(contract);
    }

    public async Task DeleteAsync(int id) => await _repo.DeleteAsync(id);
}
