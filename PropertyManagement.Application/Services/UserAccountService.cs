using System;
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
            throw new InvalidOperationException("Role must be Admin or AgencyOwner");

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

    private static string NormalizeRole(string role)
    {
        return role.Trim().ToLowerInvariant() switch
        {
            "admin" => "Admin",
            "agencyowner" => "AgencyOwner",
            _ => role.Trim()
        };
    }

    private static bool IsStaffRole(string role)
        => role.Equals("Admin", StringComparison.OrdinalIgnoreCase)
            || role.Equals("AgencyOwner", StringComparison.OrdinalIgnoreCase);
}
