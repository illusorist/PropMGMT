using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PropertyManagement.Application.DTOs.ResidentialSeeker;
using PropertyManagement.Application.Interfaces;
using PropertyManagement.Domain.Entities;

namespace PropertyManagement.Application.Services;

public class ResidentialSeekerService
{
    private readonly IResidentialSeekerRepository _repo;

    public ResidentialSeekerService(IResidentialSeekerRepository repo)
    {
        _repo = repo;
    }

    public async Task<ResidentialSeekerSearchResultDto> SearchAsync(ResidentialSeekerSearchQueryDto query)
    {
        var (items, total) = await _repo.SearchAsync(
            query.Q,
            query.Status,
            query.Employee,
            query.Page,
            query.PageSize,
            query.SortBy,
            query.SortDir);

        var page = query.Page <= 0 ? 1 : query.Page;
        var pageSize = query.PageSize <= 0 ? 20 : Math.Min(query.PageSize, 100);

        return new ResidentialSeekerSearchResultDto
        {
            Total = total,
            Page = page,
            PageSize = pageSize,
            Items = items.Select(MapDto).ToList()
        };
    }

    public async Task<ResidentialSeekerDto?> GetByIdAsync(int id)
    {
        var entity = await _repo.GetByIdAsync(id);
        return entity == null ? null : MapDto(entity);
    }

    public async Task<ResidentialSeekerDto> CreateAsync(ResidentialSeekerUpsertDto dto)
    {
        var entity = new ResidentialSeeker
        {
            SerialNumber = TrimValue(dto.SerialNumber),
            RequestDate = TrimValue(dto.RequestDate),
            Status = TrimValue(dto.Status),
            Employee = TrimValue(dto.Employee),
            Receiver = TrimValue(dto.Receiver),
            SourceChannel = TrimValue(dto.SourceChannel),
            Mobile = TrimValue(dto.Mobile),
            FullName = TrimValue(dto.FullName),
            Nationality = TrimValue(dto.Nationality),
            Profession = TrimValue(dto.Profession),
            FamilyCount = TrimValue(dto.FamilyCount),
            RequestDescription = TrimValue(dto.RequestDescription),
            MaxBudget = TrimValue(dto.MaxBudget),
            PaymentType = TrimValue(dto.PaymentType),
            PreferredLocation = TrimValue(dto.PreferredLocation),
            Notes = TrimValue(dto.Notes)
        };

        await _repo.AddAsync(entity);
        return MapDto(entity);
    }

    public async Task<ResidentialSeekerDto> UpdateAsync(int id, ResidentialSeekerUpsertDto dto)
    {
        var entity = await _repo.GetByIdAsync(id)
            ?? throw new KeyNotFoundException($"Residential seeker {id} not found");

        ApplyIfProvided(dto.SerialNumber, v => entity.SerialNumber = v);
        ApplyIfProvided(dto.RequestDate, v => entity.RequestDate = v);
        ApplyIfProvided(dto.Status, v => entity.Status = v);
        ApplyIfProvided(dto.Employee, v => entity.Employee = v);
        ApplyIfProvided(dto.Receiver, v => entity.Receiver = v);
        ApplyIfProvided(dto.SourceChannel, v => entity.SourceChannel = v);
        ApplyIfProvided(dto.Mobile, v => entity.Mobile = v);
        ApplyIfProvided(dto.FullName, v => entity.FullName = v);
        ApplyIfProvided(dto.Nationality, v => entity.Nationality = v);
        ApplyIfProvided(dto.Profession, v => entity.Profession = v);
        ApplyIfProvided(dto.FamilyCount, v => entity.FamilyCount = v);
        ApplyIfProvided(dto.RequestDescription, v => entity.RequestDescription = v);
        ApplyIfProvided(dto.MaxBudget, v => entity.MaxBudget = v);
        ApplyIfProvided(dto.PaymentType, v => entity.PaymentType = v);
        ApplyIfProvided(dto.PreferredLocation, v => entity.PreferredLocation = v);
        ApplyIfProvided(dto.Notes, v => entity.Notes = v);

        entity.UpdatedAt = DateTime.UtcNow;
        await _repo.UpdateAsync(entity);
        return MapDto(entity);
    }

    public async Task DeleteAsync(int id) => await _repo.DeleteAsync(id);

    private static string? TrimValue(string? value) => value?.Trim();

    private static void ApplyIfProvided(string? incoming, Action<string?> apply)
    {
        if (incoming == null) return;
        apply(TrimValue(incoming));
    }

    private static ResidentialSeekerDto MapDto(ResidentialSeeker entity)
    {
        return new ResidentialSeekerDto
        {
            Id = entity.Id,
            SerialNumber = entity.SerialNumber,
            RequestDate = entity.RequestDate,
            Status = entity.Status,
            Employee = entity.Employee,
            Receiver = entity.Receiver,
            SourceChannel = entity.SourceChannel,
            Mobile = entity.Mobile,
            FullName = entity.FullName,
            Nationality = entity.Nationality,
            Profession = entity.Profession,
            FamilyCount = entity.FamilyCount,
            RequestDescription = entity.RequestDescription,
            MaxBudget = entity.MaxBudget,
            PaymentType = entity.PaymentType,
            PreferredLocation = entity.PreferredLocation,
            Notes = entity.Notes,
            CreatedAt = entity.CreatedAt,
            UpdatedAt = entity.UpdatedAt
        };
    }
}
