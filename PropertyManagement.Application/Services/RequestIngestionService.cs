using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using PropertyManagement.Application.DTOs.Integrations;
using PropertyManagement.Application.Interfaces;
using PropertyManagement.Domain.Entities;

namespace PropertyManagement.Application.Services;

public class RequestIngestionService
{
    private readonly IRequestRecordRepository _repo;

    public RequestIngestionService(IRequestRecordRepository repo)
    {
        _repo = repo;
    }

    public async Task<RequestIngestResponseDto> IngestAsync(RequestIngestDto dto)
    {
        if (string.IsNullOrWhiteSpace(dto.FullName))
            throw new InvalidOperationException("FullName is required");

        if (string.IsNullOrWhiteSpace(dto.MobileNumber))
            throw new InvalidOperationException("MobileNumber is required");

        if (string.IsNullOrWhiteSpace(dto.RequestType))
            throw new InvalidOperationException("RequestType is required");

        var entity = new RequestRecord
        {
            RequestDate = dto.RequestDate,
            Status = dto.Status?.Trim() ?? string.Empty,
            Employee = dto.Employee?.Trim() ?? string.Empty,
            Via = dto.Via?.Trim() ?? string.Empty,
            FullName = dto.FullName.Trim(),
            Nationality = dto.Nationality?.Trim() ?? string.Empty,
            Profession = dto.Profession?.Trim() ?? string.Empty,
            BedroomCount = dto.BedroomCount,
            MobileNumber = dto.MobileNumber.Trim(),
            RequestType = dto.RequestType.Trim(),
            MaxBudget = dto.MaxBudget,
            PaymentType = dto.PaymentType?.Trim() ?? string.Empty,
            Location = dto.Location?.Trim() ?? string.Empty,
            Notes = dto.Notes?.Trim() ?? string.Empty,
        };

        await _repo.AddAsync(entity);

        return new RequestIngestResponseDto
        {
            Id = entity.Id,
            Message = "Request row ingested successfully."
        };
    }

    public async Task<RequestSearchResultDto> SearchAsync(RequestSearchQueryDto query)
    {
        var (items, total) = await _repo.SearchAsync(
            query.Q,
            query.Status,
            query.Employee,
            query.RequestType,
            query.FromDate,
            query.ToDate,
            query.Page,
            query.PageSize,
            query.SortBy,
            query.SortDir);

        return new RequestSearchResultDto
        {
            Total = total,
            Page = query.Page <= 0 ? 1 : query.Page,
            PageSize = query.PageSize <= 0 ? 20 : Math.Min(query.PageSize, 100),
            Items = items.Select(MapListItem).ToList()
        };
    }

    public async Task<RequestDetailsDto?> GetByIdAsync(int id)
    {
        var entity = await _repo.GetByIdAsync(id);
        return entity == null ? null : MapDetails(entity);
    }

    private static RequestListItemDto MapListItem(RequestRecord entity)
    {
        return new RequestListItemDto
        {
            Id = entity.Id,
            RequestDate = entity.RequestDate,
            Status = entity.Status,
            Employee = entity.Employee,
            FullName = entity.FullName,
            MobileNumber = entity.MobileNumber,
            RequestType = entity.RequestType,
            Location = entity.Location,
            CreatedAt = entity.CreatedAt
        };
    }

    private static RequestDetailsDto MapDetails(RequestRecord entity)
    {
        return new RequestDetailsDto
        {
            Id = entity.Id,
            RequestDate = entity.RequestDate,
            Status = entity.Status,
            Employee = entity.Employee,
            Via = entity.Via,
            FullName = entity.FullName,
            Nationality = entity.Nationality,
            Profession = entity.Profession,
            BedroomCount = entity.BedroomCount,
            MobileNumber = entity.MobileNumber,
            RequestType = entity.RequestType,
            MaxBudget = entity.MaxBudget,
            PaymentType = entity.PaymentType,
            Location = entity.Location,
            Notes = entity.Notes,
            CreatedAt = entity.CreatedAt,
        };
    }
}
