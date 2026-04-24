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

    public OwnerAccountService(IUserRepository userRepo, IOwnerRepository ownerRepo)
    {
        _userRepo = userRepo;
        _ownerRepo = ownerRepo;
    }

    public async Task CreateAccountAsync(int ownerId, OwnerAccountCreateDto dto)
    {
        var owner = await _ownerRepo.GetByIdAsync(ownerId)
            ?? throw new KeyNotFoundException($"Owner {ownerId} not found");
        if (owner.UserId.HasValue)
            throw new InvalidOperationException("Owner already has an account");

        var existingUser = await _userRepo.GetByUsernameAsync(dto.Username);
        if (existingUser != null)
            throw new InvalidOperationException("Username already in use");

        var user = new User
        {
            Username = dto.Username.Trim(),
            PasswordHash = BCrypt.Net.BCrypt.HashPassword(dto.Password),
            Role = "OwnerClient"
        };
        await _userRepo.AddAsync(user);

        owner.UserId = user.Id;
        owner.UpdatedAt = DateTime.UtcNow;
        await _ownerRepo.UpdateAsync(owner);
    }
}
