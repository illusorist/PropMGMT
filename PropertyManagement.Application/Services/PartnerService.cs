using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PropertyManagement.Application.DTOs.Partner;
using PropertyManagement.Application.Interfaces;
using PropertyManagement.Domain.Entities;

namespace PropertyManagement.Application.Services;

public class PartnerService
{
    private readonly IPartnerRepository _repo;
    private readonly IUserRepository _userRepo;

    public PartnerService(IPartnerRepository repo, IUserRepository userRepo)
    {
        _repo = repo;
        _userRepo = userRepo;
    }

    public async Task<List<PartnerResponseDto>> GetAllAsync()
    {
        var partners = await _repo.GetAllAsync();
        return partners.Select(MapPartner).ToList();
    }

    public async Task<PartnerResponseDto?> GetByIdAsync(Guid id)
    {
        var partner = await _repo.GetByIdAsync(id);
        return partner == null ? null : MapPartner(partner);
    }

    public async Task CreateAsync(CreatePartnerDto dto)
    {
        var partner = new Partner
        {
            FullName = dto.FullName,
            Phone = dto.Phone,
            Email = dto.Email,
            NationalId = dto.NationalId,
            Notes = dto.Notes
        };

        await _repo.AddAsync(partner);
    }

    public async Task UpdateAsync(Guid id, UpdatePartnerDto dto)
    {
        var partner = await _repo.GetByIdAsync(id)
            ?? throw new KeyNotFoundException($"Partner {id} not found");

        partner.FullName = dto.FullName;
        partner.Phone = dto.Phone;
        partner.Email = dto.Email;
        partner.NationalId = dto.NationalId;
        partner.Notes = dto.Notes;

        await _repo.UpdateAsync(partner);
    }

    public async Task DeleteAsync(Guid id)
        => await _repo.DeleteAsync(id);

    public async Task CreateAccountAsync(Guid partnerId, CreatePartnerAccountDto dto)
    {
        var partner = await _repo.GetByIdAsync(partnerId)
            ?? throw new KeyNotFoundException($"Partner {partnerId} not found");
        if (partner.UserId.HasValue)
            throw new InvalidOperationException("Partner already has an account");

        PasswordPolicy.EnsureStrong(dto.Password);

        var existingUser = await _userRepo.GetByUsernameAsync(dto.Username);
        if (existingUser != null)
            throw new InvalidOperationException("Username already in use");

        var user = new User
        {
            Username = dto.Username.Trim(),
            PasswordHash = BCrypt.Net.BCrypt.HashPassword(dto.Password),
            Role = "Partner"
        };
        await _userRepo.AddAsync(user);

        partner.UserId = user.Id;
        await _repo.UpdateAsync(partner);
    }

    private static PartnerResponseDto MapPartner(Partner partner)
    {
        return new PartnerResponseDto
        {
            Id = partner.Id,
            FullName = partner.FullName,
            Phone = partner.Phone,
            Email = partner.Email,
            NationalId = partner.NationalId,
            Notes = partner.Notes,
            UserId = partner.UserId,
            CreatedAt = partner.CreatedAt
        };
    }
}
