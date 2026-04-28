using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PropertyManagement.Application.DTOs.User;
using PropertyManagement.Application.Interfaces;
using PropertyManagement.Domain.Entities;

namespace PropertyManagement.Application.Services;

public class UserAccountService
{
    private readonly IUserRepository _userRepo;

    public UserAccountService(IUserRepository userRepo)
    {
        _userRepo = userRepo;
    }

    public async Task<UserResponseDto> CreateStaffAccountAsync(UserCreateDto dto)
    {
        var role = NormalizeRole(dto.Role);
        if (!IsStaffRole(role))
            throw new InvalidOperationException("Role must be Admin");

        PasswordPolicy.EnsureStrong(dto.Password);

        var existingUser = await _userRepo.GetByUsernameAsync(dto.Username);
        if (existingUser != null)
            throw new InvalidOperationException("Username already in use");

        var user = new User
        {
            Username = dto.Username.Trim(),
            PasswordHash = BCrypt.Net.BCrypt.HashPassword(dto.Password),
            Role = role
        };

        await _userRepo.AddAsync(user);

        return new UserResponseDto
        {
            Id = user.Id,
            Username = user.Username,
            Role = user.Role,
            CreatedAt = user.CreatedAt
        };
    }

    public async Task<List<UserResponseDto>> GetAllAsync()
    {
        var users = await _userRepo.GetAllAsync();
        return users.Select(u => new UserResponseDto
        {
            Id = u.Id,
            Username = u.Username,
            Role = u.Role,
            CreatedAt = u.CreatedAt
        }).ToList();
    }

    public async Task<UserResponseDto?> GetByIdAsync(int id)
    {
        var u = await _userRepo.GetByIdAsync(id);
        if (u == null) return null;
        return new UserResponseDto
        {
            Id = u.Id,
            Username = u.Username,
            Role = u.Role,
            CreatedAt = u.CreatedAt
        };
    }

    public async Task<UserResponseDto> UpdateAsync(int id, UserUpdateDto dto)
    {
        var user = await _userRepo.GetByIdAsync(id)
            ?? throw new KeyNotFoundException($"User {id} not found");

        var changed = false;

        if (!string.IsNullOrWhiteSpace(dto.Username))
        {
            var newUsername = dto.Username.Trim();
            var existing = await _userRepo.GetByUsernameAsync(newUsername);
            if (existing != null && existing.Id != user.Id)
                throw new InvalidOperationException("Username already in use");
            user.Username = newUsername;
            changed = true;
        }

        if (!string.IsNullOrWhiteSpace(dto.Password))
        {
            PasswordPolicy.EnsureStrong(dto.Password);
            user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(dto.Password);
            changed = true;
        }

        if (!string.IsNullOrWhiteSpace(dto.Role))
        {
            var role = NormalizeRole(dto.Role);
            if (!IsKnownRole(role))
                throw new InvalidOperationException("Unknown role");
            user.Role = role;
            changed = true;
        }

        if (!changed) throw new InvalidOperationException("No updatable field supplied");

        user.UpdatedAt = DateTime.UtcNow;
        await _userRepo.UpdateAsync(user);

        return new UserResponseDto
        {
            Id = user.Id,
            Username = user.Username,
            Role = user.Role,
            CreatedAt = user.CreatedAt
        };
    }

    public async Task DeleteAsync(int id)
    {
        await _userRepo.DeleteAsync(id);
    }

    public async Task ResetPasswordAsync(int id, string newPassword)
    {
        var user = await _userRepo.GetByIdAsync(id)
            ?? throw new KeyNotFoundException($"User {id} not found");

        PasswordPolicy.EnsureStrong(newPassword);

        user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(newPassword);
        user.UpdatedAt = DateTime.UtcNow;
        await _userRepo.UpdateAsync(user);
    }

    private static string NormalizeRole(string role)
    {
        return role.Trim().ToLowerInvariant() switch
        {
            "admin" => "Admin",
            "ownerclient" => "OwnerClient",
            _ => role.Trim()
        };
    }

    private static bool IsStaffRole(string role)
        => role.Equals("Admin", StringComparison.OrdinalIgnoreCase);

    private static bool IsKnownRole(string role)
        => role.Equals("Admin", StringComparison.OrdinalIgnoreCase)
            || role.Equals("OwnerClient", StringComparison.OrdinalIgnoreCase);
}
