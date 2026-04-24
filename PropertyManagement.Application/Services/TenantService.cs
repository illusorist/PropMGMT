using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PropertyManagement.Application.DTOs.Tenant;
using PropertyManagement.Application.Interfaces;
using PropertyManagement.Domain.Entities;

namespace PropertyManagement.Application.Services;

public class TenantService
{
    private readonly ITenantRepository _repo;
    public TenantService(ITenantRepository repo) => _repo = repo;

    public async Task<List<TenantResponseDto>> GetAllAsync()
    {
        var tenants = await _repo.GetAllAsync();
        return tenants.Select(t => new TenantResponseDto
        {
            Id = t.Id,
            FullName = t.FullName,
            Phone = t.Phone,
            Email = t.Email,
            NationalId = t.NationalId,
            CreatedAt = t.CreatedAt
        }).ToList();
    }

    public async Task<TenantResponseDto?> GetByIdAsync(int id)
    {
        var t = await _repo.GetByIdAsync(id);
        if (t == null) return null;
        return new TenantResponseDto
        {
            Id = t.Id,
            FullName = t.FullName,
            Phone = t.Phone,
            Email = t.Email,
            NationalId = t.NationalId,
            CreatedAt = t.CreatedAt
        };
    }

    public async Task CreateAsync(TenantCreateDto dto)
    {
        var tenant = new Tenant
        {
            FullName = dto.FullName,
            Phone = dto.Phone,
            Email = dto.Email,
            NationalId = dto.NationalId
        };
        await _repo.AddAsync(tenant);
    }

    public async Task UpdateAsync(int id, TenantCreateDto dto)
    {
        var tenant = await _repo.GetByIdAsync(id)
            ?? throw new KeyNotFoundException($"Tenant {id} not found");
        tenant.FullName = dto.FullName;
        tenant.Phone = dto.Phone;
        tenant.Email = dto.Email;
        tenant.NationalId = dto.NationalId;
        tenant.UpdatedAt = DateTime.UtcNow;
        await _repo.UpdateAsync(tenant);
    }

    public async Task DeleteAsync(int id) => await _repo.DeleteAsync(id);
}
