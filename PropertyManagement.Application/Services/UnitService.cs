using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PropertyManagement.Application.DTOs.Unit;
using PropertyManagement.Application.Interfaces;
using PropertyManagement.Domain.Entities;

namespace PropertyManagement.Application.Services;

public class UnitService
{
    private readonly IUnitRepository _repo;
    public UnitService(IUnitRepository repo) => _repo = repo;

    public async Task<List<UnitResponseDto>> GetAllAsync()
    {
        var units = await _repo.GetAllAsync();
        return units.Select(u => new UnitResponseDto
        {
            Id = u.Id,
            PropertyId = u.PropertyId,
            UnitNumber = u.UnitNumber,
            Floor = u.Floor,
            Area = u.Area,
            BaseRent = u.BaseRent,
            Status = u.Status,
            CreatedAt = u.CreatedAt
        }).ToList();
    }

    public async Task<UnitResponseDto?> GetByIdAsync(int id)
    {
        var u = await _repo.GetByIdAsync(id);
        if (u == null) return null;
        return new UnitResponseDto
        {
            Id = u.Id,
            PropertyId = u.PropertyId,
            UnitNumber = u.UnitNumber,
            Floor = u.Floor,
            Area = u.Area,
            BaseRent = u.BaseRent,
            Status = u.Status,
            CreatedAt = u.CreatedAt
        };
    }

    public async Task CreateAsync(UnitCreateDto dto)
    {
        var unit = new Unit
        {
            PropertyId = dto.PropertyId,
            UnitNumber = dto.UnitNumber,
            Floor = dto.Floor,
            Area = dto.Area,
            BaseRent = dto.BaseRent,
            Status = dto.Status
        };
        await _repo.AddAsync(unit);
    }

    public async Task UpdateAsync(int id, UnitCreateDto dto)
    {
        var unit = await _repo.GetByIdAsync(id)
            ?? throw new KeyNotFoundException($"Unit {id} not found");
        unit.PropertyId = dto.PropertyId;
        unit.UnitNumber = dto.UnitNumber;
        unit.Floor = dto.Floor;
        unit.Area = dto.Area;
        unit.BaseRent = dto.BaseRent;
        unit.Status = dto.Status;
        unit.UpdatedAt = DateTime.UtcNow;
        await _repo.UpdateAsync(unit);
    }

    public async Task DeleteAsync(int id) => await _repo.DeleteAsync(id);
}
