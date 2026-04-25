using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PropertyManagement.Application.DTOs.Lead;
using PropertyManagement.Application.Interfaces;
using PropertyManagement.Domain.Entities;
using PropertyManagement.Domain.Enums;

namespace PropertyManagement.Application.Services;

public class LeadService
{
    private readonly ILeadRepository _leadRepo;
    private readonly IPropertyRepository _propertyRepo;
    private readonly IUserRepository _userRepo;

    public LeadService(ILeadRepository leadRepo, IPropertyRepository propertyRepo, IUserRepository userRepo)
    {
        _leadRepo = leadRepo;
        _propertyRepo = propertyRepo;
        _userRepo = userRepo;
    }

    public async Task<int> CreatePublicAsync(LeadCreateDto dto)
    {
        if (dto.PropertyId.HasValue)
        {
            var property = await _propertyRepo.GetByIdAsync(dto.PropertyId.Value);
            if (property == null)
                throw new KeyNotFoundException($"Property {dto.PropertyId.Value} not found");
        }

        var lead = new Lead
        {
            PropertyId = dto.PropertyId,
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

    public async Task<List<LeadResponseDto>> GetAllAsync(LeadIntent? intent, LeadStatus? status)
    {
        var leads = await _leadRepo.GetAllWithDetailsAsync(intent, status);
        return leads.Select(Map).ToList();
    }

    public async Task<LeadResponseDto?> GetByIdAsync(int id)
    {
        var lead = await _leadRepo.GetByIdWithDetailsAsync(id);
        return lead == null ? null : Map(lead);
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

    private static LeadResponseDto Map(Lead lead)
    {
        return new LeadResponseDto
        {
            Id = lead.Id,
            PropertyId = lead.PropertyId,
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
            CreatedAt = lead.CreatedAt
        };
    }
}