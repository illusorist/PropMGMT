using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using PropertyManagement.Application.DTOs.Owner;
using PropertyManagement.Application.Interfaces;
using PropertyManagement.Domain.Entities;

namespace PropertyManagement.Application.Services;

public class OwnerAccountService
{
    private readonly IUserRepository _userRepo;
    private readonly IOwnerRepository _ownerRepo;
    private readonly IPartnerRepository _partnerRepo;

    public OwnerAccountService(IUserRepository userRepo, IOwnerRepository ownerRepo, IPartnerRepository partnerRepo)
    {
        _userRepo = userRepo;
        _ownerRepo = ownerRepo;
        _partnerRepo = partnerRepo;
    }

    public async Task CreateAccountAsync(int ownerId, OwnerAccountCreateDto dto)
    {
        var owner = await _ownerRepo.GetByIdAsync(ownerId)
            ?? throw new KeyNotFoundException($"Owner {ownerId} not found");
        if (owner.UserId.HasValue)
            throw new InvalidOperationException("Owner already has an account");

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

        var partner = new Partner
        {
            FullName = owner.FullName,
            Phone = owner.Phone,
            Email = owner.Email,
            NationalId = owner.NationalId,
            UserId = user.Id
        };
        await _partnerRepo.AddAsync(partner);

        owner.UserId = user.Id;
        owner.UpdatedAt = DateTime.UtcNow;
        await _ownerRepo.UpdateAsync(owner);
    }
}
