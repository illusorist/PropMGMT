using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PropertyManagement.Application.DTOs.Property;
using PropertyManagement.Application.Interfaces;
using PropertyManagement.Domain.Entities;

namespace PropertyManagement.Application.Services;

public class PropertyService
{
    private readonly IPropertyRepository _repo;
    public PropertyService(IPropertyRepository repo) => _repo = repo;

    public async Task<List<PropertyResponseDto>> GetAllAsync()
    {
        var properties = await _repo.GetAllAsync();
        return properties.Select(p => new PropertyResponseDto
        {
            Id = p.Id,
            OwnerId = p.OwnerId,
            Name = p.Name,
            Address = p.Address,
            Type = p.Type,
            CreatedAt = p.CreatedAt
        }).ToList();
    }

    public async Task<PropertyResponseDto?> GetByIdAsync(int id)
    {
        var p = await _repo.GetByIdAsync(id);
        if (p == null) return null;
        return new PropertyResponseDto
        {
            Id = p.Id,
            OwnerId = p.OwnerId,
            Name = p.Name,
            Address = p.Address,
            Type = p.Type,
            CreatedAt = p.CreatedAt
        };
    }

    public async Task CreateAsync(PropertyCreateDto dto)
    {
        var property = new Property
        {
            OwnerId = dto.OwnerId,
            Name = dto.Name,
            Address = dto.Address,
            Type = dto.Type
        };
        await _repo.AddAsync(property);
    }

    public async Task UpdateAsync(int id, PropertyCreateDto dto)
    {
        var property = await _repo.GetByIdAsync(id)
            ?? throw new KeyNotFoundException($"Property {id} not found");
        property.OwnerId = dto.OwnerId;
        property.Name = dto.Name;
        property.Address = dto.Address;
        property.Type = dto.Type;
        property.UpdatedAt = DateTime.UtcNow;
        await _repo.UpdateAsync(property);
    }

    public async Task DeleteAsync(int id) => await _repo.DeleteAsync(id);
}
