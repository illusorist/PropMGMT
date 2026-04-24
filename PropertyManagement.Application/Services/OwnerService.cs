using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PropertyManagement.Application.DTOs.Owner;
using PropertyManagement.Application.Interfaces;
using PropertyManagement.Domain.Entities;

namespace PropertyManagement.Application.Services;

public class OwnerService
{
    private readonly IOwnerRepository _repo;
    public OwnerService(IOwnerRepository repo) => _repo = repo;

    public async Task<List<OwnerResponseDto>> GetAllAsync()
    {
        var owners = await _repo.GetAllAsync();
        return owners.Select(o => new OwnerResponseDto
        {
            Id = o.Id,
            FullName = o.FullName,
            Phone = o.Phone,
            Email = o.Email,
            NationalId = o.NationalId,
            CreatedAt = o.CreatedAt
        }).ToList();
    }

    public async Task<OwnerResponseDto?> GetByIdAsync(int id)
    {
        var o = await _repo.GetByIdAsync(id);
        if (o == null) return null;
        return new OwnerResponseDto
        {
            Id = o.Id,
            FullName = o.FullName,
            Phone = o.Phone,
            Email = o.Email,
            NationalId = o.NationalId,
            CreatedAt = o.CreatedAt
        };
    }

    public async Task CreateAsync(OwnerCreateDto dto)
    {
        var owner = new Owner
        {
            FullName = dto.FullName,
            Phone = dto.Phone,
            Email = dto.Email,
            NationalId = dto.NationalId
        };
        await _repo.AddAsync(owner);
    }

    public async Task UpdateAsync(int id, OwnerCreateDto dto)
    {
        var owner = await _repo.GetByIdAsync(id)
            ?? throw new KeyNotFoundException($"Owner {id} not found");
        owner.FullName = dto.FullName;
        owner.Phone = dto.Phone;
        owner.Email = dto.Email;
        owner.NationalId = dto.NationalId;
        owner.UpdatedAt = DateTime.UtcNow;
        await _repo.UpdateAsync(owner);
    }

    public async Task DeleteAsync(int id) => await _repo.DeleteAsync(id);
}
