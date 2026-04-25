using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using PropertyManagement.Application.DTOs.Lead;
using PropertyManagement.Application.Interfaces;
using PropertyManagement.Domain.Entities;
using PropertyManagement.Domain.Enums;

namespace PropertyManagement.Application.Services;

public class LeadService
{
    private readonly ILeadRepository _leadRepo;
    private readonly ILeadImageRepository _leadImageRepo;
    private readonly ILeadImageStorage _leadImageStorage;
    private readonly IPropertyRepository _propertyRepo;
    private readonly IPropertyImageRepository _propertyImageRepo;
    private readonly IPropertyImageStorage _propertyImageStorage;
    private readonly IOwnerRepository _ownerRepo;
    private readonly IUserRepository _userRepo;

    public LeadService(
        ILeadRepository leadRepo,
        ILeadImageRepository leadImageRepo,
        ILeadImageStorage leadImageStorage,
        IPropertyRepository propertyRepo,
        IPropertyImageRepository propertyImageRepo,
        IPropertyImageStorage propertyImageStorage,
        IOwnerRepository ownerRepo,
        IUserRepository userRepo)
    {
        _leadRepo = leadRepo;
        _leadImageRepo = leadImageRepo;
        _leadImageStorage = leadImageStorage;
        _propertyRepo = propertyRepo;
        _propertyImageRepo = propertyImageRepo;
        _propertyImageStorage = propertyImageStorage;
        _ownerRepo = ownerRepo;
        _userRepo = userRepo;
    }

    public async Task<int> CreatePublicAsync(LeadCreateDto dto)
    {
        var lead = new Lead
        {
            PropertyId = dto.PropertyId,
            PropertyName = dto.PropertyName,
            PropertyAddress = dto.PropertyAddress,
            PropertyType = dto.PropertyType,
            OwnerNationalId = dto.OwnerNationalId,
            FullName = dto.FullName,
            Phone = dto.Phone,
            Email = dto.Email,
            Notes = dto.Notes,
            Intent = dto.Intent,
            PreferredContactAt = dto.PreferredContactAt,
            Status = LeadStatus.New
        };

        await _leadRepo.AddAsync(lead);
        return lead.Id;
    }

    public async Task<LeadImageResponseDto> AddImageAsync(
        int leadId,
        Stream stream,
        string originalFileName,
        string contentType,
        long sizeBytes,
        CancellationToken cancellationToken = default)
    {
        var lead = await _leadRepo.GetByIdAsync(leadId)
            ?? throw new KeyNotFoundException($"Lead {leadId} not found");

        var existingImages = await _leadImageRepo.GetByLeadIdAsync(leadId);
        var stored = await _leadImageStorage.SaveAsync(leadId, stream, originalFileName, contentType, sizeBytes, cancellationToken);
        var image = new LeadImage
        {
            LeadId = leadId,
            StoredFileName = stored.StoredFileName,
            OriginalFileName = originalFileName,
            RelativePath = stored.RelativePath,
            MimeType = contentType,
            SizeBytes = sizeBytes,
            SortOrder = (await _leadImageRepo.GetMaxSortOrderAsync(leadId)) + 1,
            IsPrimary = existingImages.Count == 0
        };

        if (image.IsPrimary)
        {
            foreach (var existingImage in existingImages.Where(i => i.IsPrimary))
                existingImage.IsPrimary = false;
            if (existingImages.Count > 0)
                await _leadImageRepo.UpdateAsync(existingImages.First());
        }

        await _leadImageRepo.AddAsync(image);
        return MapLeadImage(image);
    }

    public async Task<List<LeadResponseDto>> GetAllAsync(LeadIntent? intent, LeadStatus? status)
    {
        var leads = await _leadRepo.GetAllWithDetailsAsync(intent, status);
        return leads.Select(MapLead).ToList();
    }

    public async Task<LeadResponseDto?> GetByIdAsync(int id)
    {
        var lead = await _leadRepo.GetByIdWithDetailsAndImagesAsync(id);
        return lead == null ? null : MapLead(lead);
    }

    public async Task<LeadImageFileDto> GetImageFileAsync(int leadId, int imageId)
    {
        var image = await _leadImageRepo.GetByIdForLeadAsync(leadId, imageId)
            ?? throw new KeyNotFoundException($"Image {imageId} not found for lead {leadId}");

        return new LeadImageFileDto
        {
            Stream = File.OpenRead(_leadImageStorage.GetPhysicalPath(image.RelativePath)),
            FileName = image.OriginalFileName,
            ContentType = image.MimeType
        };
    }

    public async Task<int> ApproveAsync(int leadId)
    {
        var lead = await _leadRepo.GetByIdWithDetailsAndImagesAsync(leadId)
            ?? throw new KeyNotFoundException($"Lead {leadId} not found");

        if (lead.Status == LeadStatus.ClosedWon)
            throw new InvalidOperationException("Lead has already been approved");

        var copiedPropertyFiles = new List<string>();
        Owner? owner = null;
        var createdOwner = false;
        var createdPropertyId = 0;

        try
        {
            owner = await _ownerRepo.GetByNationalIdAsync(lead.OwnerNationalId);
            if (owner == null)
            {
                owner = new Owner
                {
                    FullName = lead.FullName,
                    Phone = lead.Phone,
                    Email = lead.Email,
                    NationalId = lead.OwnerNationalId
                };
                await _ownerRepo.AddAsync(owner);
                createdOwner = true;
            }
            else
            {
                owner.FullName = lead.FullName;
                owner.Phone = lead.Phone;
                owner.Email = lead.Email;
                owner.UpdatedAt = DateTime.UtcNow;
                await _ownerRepo.UpdateAsync(owner);
            }

            var property = new Property
            {
                OwnerId = owner.Id,
                Name = lead.PropertyName,
                Address = lead.PropertyAddress,
                Type = lead.PropertyType,
                Status = PropertyStatus.Approved
            };
            await _propertyRepo.AddAsync(property);
            createdPropertyId = property.Id;

            var leadImages = lead.Images.OrderBy(i => i.SortOrder).ThenBy(i => i.Id).ToList();
            foreach (var draftImage in leadImages)
            {
                await using var source = File.OpenRead(_leadImageStorage.GetPhysicalPath(draftImage.RelativePath));
                var stored = await _propertyImageStorage.SaveAsync(property.Id, source, draftImage.OriginalFileName, draftImage.MimeType, draftImage.SizeBytes);
                copiedPropertyFiles.Add(stored.RelativePath);

                var propertyImage = new PropertyImage
                {
                    PropertyId = property.Id,
                    StoredFileName = stored.StoredFileName,
                    OriginalFileName = draftImage.OriginalFileName,
                    RelativePath = stored.RelativePath,
                    MimeType = draftImage.MimeType,
                    SizeBytes = draftImage.SizeBytes,
                    SortOrder = draftImage.SortOrder,
                    IsPrimary = draftImage.IsPrimary
                };

                if (propertyImage.IsPrimary)
                    await _propertyImageRepo.ClearPrimaryAsync(property.Id);

                await _propertyImageRepo.AddAsync(propertyImage);
            }

            lead.PropertyId = property.Id;
            lead.Status = LeadStatus.ClosedWon;
            lead.UpdatedAt = DateTime.UtcNow;
            await _leadRepo.UpdateAsync(lead);

            foreach (var draftImage in leadImages)
            {
                await _leadImageRepo.DeleteAsync(draftImage.Id);
                await _leadImageStorage.DeleteAsync(draftImage.RelativePath);
            }

            return property.Id;
        }
        catch
        {
            foreach (var relativePath in copiedPropertyFiles)
            {
                try
                {
                    await _propertyImageStorage.DeleteAsync(relativePath);
                }
                catch
                {
                    // best-effort cleanup
                }
            }

            if (createdPropertyId != 0)
            {
                try
                {
                    await _propertyRepo.DeleteAsync(createdPropertyId);
                }
                catch
                {
                    // best-effort cleanup
                }
            }

            if (createdOwner && owner != null)
            {
                try
                {
                    await _ownerRepo.DeleteAsync(owner.Id);
                }
                catch
                {
                    // best-effort cleanup
                }
            }

            throw;
        }
    }

    public async Task UpdateAsync(int id, LeadUpdateDto dto)
    {
        var lead = await _leadRepo.GetByIdAsync(id)
            ?? throw new KeyNotFoundException($"Lead {id} not found");

        if (dto.AssignedToUserId.HasValue)
        {
            var user = await _userRepo.GetByIdAsync(dto.AssignedToUserId.Value);
            if (user == null)
                throw new KeyNotFoundException($"User {dto.AssignedToUserId.Value} not found");
        }

        lead.Status = dto.Status;
        lead.AssignedToUserId = dto.AssignedToUserId;
        lead.Notes = dto.Notes;
        lead.LastContactedAt = dto.LastContactedAt;
        lead.PreferredContactAt = dto.PreferredContactAt;
        lead.UpdatedAt = DateTime.UtcNow;

        await _leadRepo.UpdateAsync(lead);
    }

    private static LeadResponseDto MapLead(Lead lead)
    {
        return new LeadResponseDto
        {
            Id = lead.Id,
            PropertyId = lead.PropertyId,
            PropertyName = lead.PropertyName,
            PropertyAddress = lead.PropertyAddress,
            PropertyType = lead.PropertyType,
            OwnerNationalId = lead.OwnerNationalId,
            FullName = lead.FullName,
            Phone = lead.Phone,
            Email = lead.Email,
            Notes = lead.Notes,
            Intent = lead.Intent,
            Status = lead.Status,
            PreferredContactAt = lead.PreferredContactAt,
            LastContactedAt = lead.LastContactedAt,
            AssignedToUserId = lead.AssignedToUserId,
            AssignedToUsername = lead.AssignedToUser?.Username,
            Images = lead.Images.OrderBy(i => i.SortOrder).ThenBy(i => i.Id).Select(MapLeadImage).ToList(),
            CreatedAt = lead.CreatedAt
        };
    }

    private static LeadImageResponseDto MapLeadImage(LeadImage image)
    {
        return new LeadImageResponseDto
        {
            Id = image.Id,
            LeadId = image.LeadId,
            OriginalFileName = image.OriginalFileName,
            MimeType = image.MimeType,
            SizeBytes = image.SizeBytes,
            SortOrder = image.SortOrder,
            IsPrimary = image.IsPrimary,
            FileUrl = $"/api/leads/{image.LeadId}/images/{image.Id}/file",
            CreatedAt = image.CreatedAt
        };
    }
}
