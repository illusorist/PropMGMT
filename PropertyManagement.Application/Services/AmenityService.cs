using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PropertyManagement.Application.DTOs.Amenity;
using PropertyManagement.Application.Interfaces;
using PropertyManagement.Domain.Entities;

namespace PropertyManagement.Application.Services;

public class AmenityService
{
    private readonly IAmenityRepository _repo;
    public AmenityService(IAmenityRepository repo) => _repo = repo;

    public async Task<List<AmenityResponseDto>> GetAllAsync()
    {
        var amenities = await _repo.GetAllAsync();
        return amenities.Select(a => new AmenityResponseDto
        {
            Id = a.Id,
            Name = a.Name,
            Description = a.Description,
            CreatedAt = a.CreatedAt
        }).ToList();
    }

    public async Task<AmenityResponseDto?> GetByIdAsync(int id)
    {
        var a = await _repo.GetByIdAsync(id);
        if (a == null) return null;
        return new AmenityResponseDto
        {
            Id = a.Id,
            Name = a.Name,
            Description = a.Description,
            CreatedAt = a.CreatedAt
        };
    }

    public async Task CreateAsync(AmenityCreateDto dto)
    {
        var amenity = new Amenity
        {
            Name = dto.Name.Trim(),
            NormalizedName = NormalizeName(dto.Name),
            Description = dto.Description?.Trim() ?? string.Empty
        };
        await _repo.AddAsync(amenity);
    }

    public async Task UpdateAsync(int id, AmenityCreateDto dto)
    {
        var amenity = await _repo.GetByIdAsync(id)
            ?? throw new KeyNotFoundException($"Amenity {id} not found");
        amenity.Name = dto.Name.Trim();
        amenity.NormalizedName = NormalizeName(dto.Name);
        amenity.Description = dto.Description?.Trim() ?? string.Empty;
        amenity.UpdatedAt = DateTime.UtcNow;
        await _repo.UpdateAsync(amenity);
    }

    public async Task DeleteAsync(int id) => await _repo.DeleteAsync(id);

    private static string NormalizeName(string name) => name.Trim().ToUpperInvariant();
}
