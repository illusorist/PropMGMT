using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PropertyManagement.Application.DTOs.Amenity;
using PropertyManagement.Application.DTOs.Property;
using PropertyManagement.Application.Interfaces;
using PropertyManagement.Domain.Entities;
using PropertyManagement.Domain.Enums;

namespace PropertyManagement.Application.Services;

public class PropertyService
{
    private readonly IPropertyRepository _repo;
    private readonly IAmenityRepository _amenityRepo;
    private readonly IPropertyImageStorage _propertyImageStorage;

    public PropertyService(IPropertyRepository repo, IAmenityRepository amenityRepo, IPropertyImageStorage propertyImageStorage)
    {
        _repo = repo;
        _amenityRepo = amenityRepo;
        _propertyImageStorage = propertyImageStorage;
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
            SalePrice = p.SalePrice,
            RentPrice = p.RentPrice,
            PrimaryImageUrl = GetPrimaryImageUrl(p),
            Status = p.Status,
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
            SalePrice = p.SalePrice,
            RentPrice = p.RentPrice,
            PrimaryImageUrl = GetPrimaryImageUrl(p),
            Status = p.Status,
            CreatedAt = p.CreatedAt,
            Amenities = MapAmenities(p)
        };
    }

    public async Task<List<PropertyResponseDto>> GetAllForOwnerAsync(int ownerId)
    {
        var properties = await _repo.GetAllWithAmenitiesByOwnerIdAsync(ownerId);
        return properties.Select(p => new PropertyResponseDto
        {
            Id = p.Id,
            OwnerId = p.OwnerId,
            Name = p.Name,
            Address = p.Address,
            Type = p.Type,
            SalePrice = p.SalePrice,
            RentPrice = p.RentPrice,
            PrimaryImageUrl = GetPrimaryImageUrl(p),
            Status = p.Status,
            CreatedAt = p.CreatedAt,
            Amenities = MapAmenities(p)
        }).ToList();
    }

    public async Task<PropertyResponseDto?> GetByIdForOwnerAsync(int ownerId, int id)
    {
        var p = await _repo.GetByIdWithAmenitiesByOwnerIdAsync(ownerId, id);
        if (p == null) return null;
        return new PropertyResponseDto
        {
            Id = p.Id,
            OwnerId = p.OwnerId,
            Name = p.Name,
            Address = p.Address,
            Type = p.Type,
            SalePrice = p.SalePrice,
            RentPrice = p.RentPrice,
            PrimaryImageUrl = GetPrimaryImageUrl(p),
            Status = p.Status,
            CreatedAt = p.CreatedAt,
            Amenities = MapAmenities(p)
        };
    }

    public async Task<List<PublicPropertyResponseDto>> GetPublicAvailableAsync()
    {
        var properties = await _repo.GetAllWithAmenitiesAsync();
        return properties
            .Where(p => p.Status == PropertyStatus.Approved)
            .Select(p => new PublicPropertyResponseDto
            {
                Id = p.Id,
                Name = p.Name,
                Address = p.Address,
                Type = p.Type,
                SalePrice = p.SalePrice,
                RentPrice = p.RentPrice,
                PrimaryImageUrl = GetPrimaryImageUrl(p),
                Images = p.Images
                    .OrderBy(i => i.SortOrder)
                    .ThenBy(i => i.Id)
                    .Select(i => new PublicPropertyImageDto
                    {
                        Id = i.Id,
                        OriginalFileName = i.OriginalFileName,
                        Url = _propertyImageStorage.BuildPublicUrl(i.RelativePath),
                        IsPrimary = i.IsPrimary,
                        SortOrder = i.SortOrder
                    })
                    .ToList(),
                Status = p.Status,
                CreatedAt = p.CreatedAt,
                Amenities = MapAmenities(p)
            })
            .ToList();
    }

    public async Task<int> CreateAsync(PropertyCreateDto dto)
    {
        var amenityIds = NormalizeAmenityIds(dto.AmenityIds);
        await EnsureAmenitiesExistAsync(amenityIds);
        var property = new Property
        {
            OwnerId = dto.OwnerId,
            Name = dto.Name,
            Address = dto.Address,
            Type = dto.Type,
            SalePrice = dto.SalePrice,
            RentPrice = dto.RentPrice,
            PropertyAmenities = amenityIds.Select(id => new PropertyAmenity
            {
                AmenityId = id
            }).ToList()
        };
        await _repo.AddAsync(property);
        return property.Id;
    }

    public async Task<int> CreateForOwnerAsync(int ownerId, PropertyCreateDto dto)
    {
        var amenityIds = NormalizeAmenityIds(dto.AmenityIds);
        await EnsureAmenitiesExistAsync(amenityIds);
        var property = new Property
        {
            OwnerId = ownerId,
            Name = dto.Name,
            Address = dto.Address,
            Type = dto.Type,
            SalePrice = dto.SalePrice,
            RentPrice = dto.RentPrice,
            PropertyAmenities = amenityIds.Select(id => new PropertyAmenity
            {
                AmenityId = id
            }).ToList()
        };
        await _repo.AddAsync(property);
        return property.Id;
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
        property.SalePrice = dto.SalePrice;
        property.RentPrice = dto.RentPrice;
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

    public async Task UpdateForOwnerAsync(int ownerId, int id, PropertyCreateDto dto)
    {
        var property = await _repo.GetByIdWithAmenitiesByOwnerIdAsync(ownerId, id)
            ?? throw new KeyNotFoundException($"Property {id} not found");
        var amenityIds = NormalizeAmenityIds(dto.AmenityIds);
        await EnsureAmenitiesExistAsync(amenityIds);
        property.Name = dto.Name;
        property.Address = dto.Address;
        property.Type = dto.Type;
        property.SalePrice = dto.SalePrice;
        property.RentPrice = dto.RentPrice;
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

    public async Task DeleteForOwnerAsync(int ownerId, int id)
    {
        var property = await _repo.GetByIdWithAmenitiesByOwnerIdAsync(ownerId, id)
            ?? throw new KeyNotFoundException($"Property {id} not found");
        await _repo.DeleteAsync(property.Id);
    }

    public async Task UpdateStatusAsync(int id, PropertyStatus status)
    {
        var property = await _repo.GetByIdWithAmenitiesAsync(id)
            ?? throw new KeyNotFoundException($"Property {id} not found");
        property.Status = status;
        property.UpdatedAt = DateTime.UtcNow;
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

    private string? GetPrimaryImageUrl(Property property)
    {
        var primaryImage = property.Images.OrderBy(i => i.SortOrder).ThenBy(i => i.Id).FirstOrDefault(i => i.IsPrimary)
            ?? property.Images.OrderBy(i => i.SortOrder).ThenBy(i => i.Id).FirstOrDefault();
        return primaryImage == null ? null : _propertyImageStorage.BuildPublicUrl(primaryImage.RelativePath);
    }
}
