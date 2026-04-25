using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using PropertyManagement.Application.DTOs.PropertyImage;
using PropertyManagement.Application.Interfaces;
using PropertyManagement.Domain.Entities;

namespace PropertyManagement.Application.Services;

public class PropertyImageService
{
    private readonly IPropertyImageRepository _imageRepo;
    private readonly IPropertyRepository _propertyRepo;
    private readonly IPropertyImageStorage _storage;

    public PropertyImageService(IPropertyImageRepository imageRepo, IPropertyRepository propertyRepo, IPropertyImageStorage storage)
    {
        _imageRepo = imageRepo;
        _propertyRepo = propertyRepo;
        _storage = storage;
    }

    public async Task<List<PropertyImageResponseDto>> GetByPropertyIdAsync(int propertyId, int? ownerId)
    {
        await EnsurePropertyAccessAsync(propertyId, ownerId);
        var images = await _imageRepo.GetByPropertyIdAsync(propertyId);
        return images.Select(Map).ToList();
    }

    public async Task<PropertyImageResponseDto> UploadAsync(
        int propertyId,
        int? ownerId,
        Stream stream,
        string originalFileName,
        string contentType,
        long sizeBytes,
        bool isPrimary,
        int? sortOrder,
        CancellationToken cancellationToken = default)
    {
        await EnsurePropertyAccessAsync(propertyId, ownerId);

        var stored = await _storage.SaveAsync(propertyId, stream, originalFileName, contentType, sizeBytes, cancellationToken);

        if (isPrimary)
            await _imageRepo.ClearPrimaryAsync(propertyId);

        var image = new PropertyImage
        {
            PropertyId = propertyId,
            StoredFileName = stored.StoredFileName,
            OriginalFileName = originalFileName,
            RelativePath = stored.RelativePath,
            MimeType = contentType,
            SizeBytes = sizeBytes,
            IsPrimary = isPrimary,
            SortOrder = sortOrder ?? (await _imageRepo.GetMaxSortOrderAsync(propertyId)) + 1
        };

        await _imageRepo.AddAsync(image);
        return Map(image);
    }

    public async Task DeleteAsync(int propertyId, int imageId, int? ownerId, CancellationToken cancellationToken = default)
    {
        await EnsurePropertyAccessAsync(propertyId, ownerId);
        var image = await _imageRepo.GetByIdForPropertyAsync(propertyId, imageId)
            ?? throw new KeyNotFoundException($"Image {imageId} not found for property {propertyId}");

        await _imageRepo.DeleteAsync(image.Id);
        try
        {
            await _storage.DeleteAsync(image.RelativePath, cancellationToken);
        }
        catch
        {
            // Keep API deletion idempotent even if disk cleanup fails.
        }
    }

    public async Task SetPrimaryAsync(int propertyId, int imageId, int? ownerId)
    {
        await EnsurePropertyAccessAsync(propertyId, ownerId);
        var image = await _imageRepo.GetByIdForPropertyAsync(propertyId, imageId)
            ?? throw new KeyNotFoundException($"Image {imageId} not found for property {propertyId}");

        await _imageRepo.ClearPrimaryAsync(propertyId);
        image.IsPrimary = true;
        image.UpdatedAt = DateTime.UtcNow;
        await _imageRepo.UpdateAsync(image);
    }

    public async Task ReorderAsync(int propertyId, PropertyImageReorderDto dto, int? ownerId)
    {
        await EnsurePropertyAccessAsync(propertyId, ownerId);
        var images = await _imageRepo.GetByPropertyIdAsync(propertyId);

        var existingIds = images.Select(i => i.Id).OrderBy(id => id).ToList();
        var requestedIds = dto.ImageIdsInOrder.Distinct().OrderBy(id => id).ToList();
        if (!existingIds.SequenceEqual(requestedIds))
            throw new InvalidOperationException("Image order must include all and only existing property image IDs");

        var byId = images.ToDictionary(i => i.Id);
        for (var index = 0; index < dto.ImageIdsInOrder.Count; index++)
        {
            var image = byId[dto.ImageIdsInOrder[index]];
            image.SortOrder = index + 1;
            image.UpdatedAt = DateTime.UtcNow;
        }

        await _imageRepo.UpdateRangeAsync(images);
    }

    private async Task EnsurePropertyAccessAsync(int propertyId, int? ownerId)
    {
        var property = ownerId.HasValue
            ? await _propertyRepo.GetByIdWithAmenitiesByOwnerIdAsync(ownerId.Value, propertyId)
            : await _propertyRepo.GetByIdAsync(propertyId);
        if (property == null)
            throw new KeyNotFoundException($"Property {propertyId} not found");
    }

    private PropertyImageResponseDto Map(PropertyImage image)
    {
        return new PropertyImageResponseDto
        {
            Id = image.Id,
            PropertyId = image.PropertyId,
            OriginalFileName = image.OriginalFileName,
            MimeType = image.MimeType,
            SizeBytes = image.SizeBytes,
            SortOrder = image.SortOrder,
            IsPrimary = image.IsPrimary,
            Url = _storage.BuildPublicUrl(image.RelativePath),
            CreatedAt = image.CreatedAt
        };
    }
}
