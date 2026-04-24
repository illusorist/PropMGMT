using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PropertyManagement.Application.DTOs.Amenity;
using PropertyManagement.Application.DTOs.Property;
using PropertyManagement.Application.Interfaces;
using PropertyManagement.Domain.Entities;

namespace PropertyManagement.Application.Services;

public class PropertyService
{
    private readonly IPropertyRepository _repo;
    private readonly IAmenityRepository _amenityRepo;

    public PropertyService(IPropertyRepository repo, IAmenityRepository amenityRepo)
    {
        _repo = repo;
        _amenityRepo = amenityRepo;
    }

    public async Task<List<PropertyResponseDto>> GetAllAsync()
    {
        var properties = await _repo.GetAllWithAmenitiesAsync();
        return properties.Select(p => new PropertyResponseDto
        {
            Id = p.Id,
            OwnerId = p.OwnerId,
            Name = p.Name,
            Address = p.Address,
            Type = p.Type,
            CreatedAt = p.CreatedAt,
            Amenities = MapAmenities(p)
        }).ToList();
    }

    public async Task<PropertyResponseDto?> GetByIdAsync(int id)
    {
        var p = await _repo.GetByIdWithAmenitiesAsync(id);
        if (p == null) return null;
        return new PropertyResponseDto
        {
            Id = p.Id,
            OwnerId = p.OwnerId,
            Name = p.Name,
            Address = p.Address,
            Type = p.Type,
            CreatedAt = p.CreatedAt,
            Amenities = MapAmenities(p)
        };
    }

    public async Task CreateAsync(PropertyCreateDto dto)
    {
        var amenityIds = NormalizeAmenityIds(dto.AmenityIds);
        await EnsureAmenitiesExistAsync(amenityIds);
        var property = new Property
        {
            OwnerId = dto.OwnerId,
            Name = dto.Name,
            Address = dto.Address,
            Type = dto.Type,
            PropertyAmenities = amenityIds.Select(id => new PropertyAmenity
            {
                AmenityId = id
            }).ToList()
        };
        await _repo.AddAsync(property);
    }

    public async Task UpdateAsync(int id, PropertyCreateDto dto)
    {
        var property = await _repo.GetByIdWithAmenitiesAsync(id)
            ?? throw new KeyNotFoundException($"Property {id} not found");
        var amenityIds = NormalizeAmenityIds(dto.AmenityIds);
        await EnsureAmenitiesExistAsync(amenityIds);
        property.OwnerId = dto.OwnerId;
        property.Name = dto.Name;
        property.Address = dto.Address;
        property.Type = dto.Type;
        property.UpdatedAt = DateTime.UtcNow;
        property.PropertyAmenities.Clear();
        foreach (var amenityId in amenityIds)
        {
            property.PropertyAmenities.Add(new PropertyAmenity
            {
                PropertyId = property.Id,
                AmenityId = amenityId
            });
        }
        await _repo.UpdateAsync(property);
    }

    public async Task DeleteAsync(int id) => await _repo.DeleteAsync(id);

    private static List<int> NormalizeAmenityIds(List<int> amenityIds)
    {
        return amenityIds
            .Where(id => id > 0)
            .Distinct()
            .ToList();
    }

    private async Task EnsureAmenitiesExistAsync(List<int> amenityIds)
    {
        if (amenityIds.Count == 0) return;
        var amenities = await _amenityRepo.GetByIdsAsync(amenityIds);
        if (amenities.Count != amenityIds.Count)
        {
            throw new KeyNotFoundException("One or more amenities were not found");
        }
    }

    private static List<AmenityResponseDto> MapAmenities(Property property)
    {
        return property.PropertyAmenities
            .Where(pa => pa.Amenity != null)
            .Select(pa => new AmenityResponseDto
            {
                Id = pa.Amenity.Id,
                Name = pa.Amenity.Name,
                Description = pa.Amenity.Description,
                CreatedAt = pa.Amenity.CreatedAt
            })
            .ToList();
    }
}
