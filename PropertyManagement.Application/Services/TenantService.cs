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
    private readonly IPropertyRepository _propertyRepo;

    public TenantService(ITenantRepository repo, IPropertyRepository propertyRepo)
    {
        _repo = repo;
        _propertyRepo = propertyRepo;
    }

    public async Task<List<TenantResponseDto>> GetAllAsync()
    {
        var tenants = await _repo.GetAllAsync();
        var properties = await _propertyRepo.GetAllAsync();
        var propertyById = properties.ToDictionary(property => property.Id);

        return tenants.Select(t => new TenantResponseDto
        {
            Id = t.Id,
            PropertyId = t.PropertyId,
            PropertyName = t.PropertyId.HasValue && propertyById.TryGetValue(t.PropertyId.Value, out var property)
                ? property.Name
                : null,
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

        Property? property = null;
        if (t.PropertyId.HasValue)
        {
            property = await _propertyRepo.GetByIdAsync(t.PropertyId.Value);
        }

        return new TenantResponseDto
        {
            Id = t.Id,
            PropertyId = t.PropertyId,
            PropertyName = property?.Name,
            FullName = t.FullName,
            Phone = t.Phone,
            Email = t.Email,
            NationalId = t.NationalId,
            CreatedAt = t.CreatedAt
        };
    }

    public async Task CreateAsync(TenantCreateDto dto)
    {
        var propertyId = await ResolvePropertyIdAsync(dto.PropertyId);
        var tenant = new Tenant
        {
            PropertyId = propertyId,
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
        var propertyId = await ResolvePropertyIdAsync(dto.PropertyId);
        tenant.PropertyId = propertyId;
        tenant.FullName = dto.FullName;
        tenant.Phone = dto.Phone;
        tenant.Email = dto.Email;
        tenant.NationalId = dto.NationalId;
        tenant.UpdatedAt = DateTime.UtcNow;
        await _repo.UpdateAsync(tenant);
    }

    public async Task DeleteAsync(int id) => await _repo.DeleteAsync(id);

    private async Task<int> ResolvePropertyIdAsync(int? propertyId)
    {
        if (!propertyId.HasValue || propertyId.Value <= 0)
            throw new InvalidOperationException("Property is required");

        var property = await _propertyRepo.GetByIdAsync(propertyId.Value);
        if (property == null)
            throw new KeyNotFoundException($"Property {propertyId.Value} not found");

        return property.Id;
    }
}
