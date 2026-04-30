using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using PropertyManagement.Application.DTOs.User;
using PropertyManagement.Application.Interfaces;
using PropertyManagement.Domain.Entities;

namespace PropertyManagement.Application.Services;

public class UserAccountService
{
    private static readonly HashSet<string> EmployeeVisibleScreens = new(StringComparer.OrdinalIgnoreCase)
    {
        "/app",
        "/app/owners",
        "/app/properties",
        "/app/amenities",
        "/app/tenants",
        "/app/contracts",
        "/app/payments",
        "/app/buyers",
        "/app/sales",
        "/app/leads"
    };

    private readonly IUserRepository _userRepo;
    private readonly IOwnerRepository _ownerRepo;
    private readonly ILeadRepository _leadRepo;

    public UserAccountService(IUserRepository userRepo, IOwnerRepository ownerRepo, ILeadRepository leadRepo)
    {
        _userRepo = userRepo;
        _ownerRepo = ownerRepo;
        _leadRepo = leadRepo;
    }

    public async Task<UserResponseDto> CreateStaffAccountAsync(UserCreateDto dto)
    {
        var role = NormalizeRole(dto.Role);
        if (!IsStaffRole(role))
            throw new InvalidOperationException("Role must be Admin or Employee");

        var screenPermissions = ResolveScreenPermissions(role, dto.ScreenPermissions, requireExisting: true);

        PasswordPolicy.EnsureStrong(dto.Password);

        var existingUser = await _userRepo.GetByUsernameAsync(dto.Username);
        if (existingUser != null)
            throw new InvalidOperationException("Username already in use");

        var user = new User
        {
            Username = dto.Username.Trim(),
            PasswordHash = BCrypt.Net.BCrypt.HashPassword(dto.Password),
            Role = role,
            ScreenPermissionsJson = SerializeScreenPermissions(screenPermissions)
        };

        await _userRepo.AddAsync(user);

        return await MapUserAsync(user);
    }

    public async Task<List<UserResponseDto>> GetAllAsync()
    {
        var users = await _userRepo.GetAllAsync();
        var owners = await _ownerRepo.GetAllAsync();
        var ownerByUserId = owners
            .Where(owner => owner.UserId.HasValue)
            .ToDictionary(owner => owner.UserId!.Value);

        return users.Select(user => MapUser(user, ownerByUserId.TryGetValue(user.Id, out var owner) ? owner : null)).ToList();
    }

    public async Task<UserResponseDto?> GetByIdAsync(int id)
    {
        var u = await _userRepo.GetByIdAsync(id);
        if (u == null) return null;
        return await MapUserAsync(u);
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

        if (dto.ScreenPermissions != null)
        {
            changed = true;
        }

        if (!changed) throw new InvalidOperationException("No updatable field supplied");

        var screenPermissions = ResolveScreenPermissions(
            user.Role,
            dto.ScreenPermissions,
            requireExisting: user.Role.Equals("Employee", StringComparison.OrdinalIgnoreCase) && dto.Role is not null);
        user.ScreenPermissionsJson = SerializeScreenPermissions(screenPermissions);

        user.UpdatedAt = DateTime.UtcNow;
        await _userRepo.UpdateAsync(user);

        return await MapUserAsync(user);
    }

    public async Task DeleteAsync(int id)
    {
        var user = await _userRepo.GetByIdAsync(id)
            ?? throw new KeyNotFoundException($"User {id} not found");

        var linkedOwner = await _ownerRepo.GetByUserIdAsync(id);
        if (linkedOwner != null)
            throw new InvalidOperationException($"Cannot delete user '{user.Username}' because it is linked to owner account '{linkedOwner.FullName}'. Delete the owner's account first or unlink the user.");

        var assignedLeads = await _leadRepo.GetAllWithDetailsAsync(null, null);
        if (assignedLeads.Any(l => l.AssignedToUserId == id))
            throw new InvalidOperationException($"Cannot delete user '{user.Username}' because it is assigned to one or more leads. Unassign the user from all leads first.");

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
            "employee" => "Employee",
            "ownerclient" => "OwnerClient",
            _ => role.Trim()
        };
    }

    private async Task<UserResponseDto> MapUserAsync(User user)
    {
        var owner = await _ownerRepo.GetByUserIdAsync(user.Id);
        return MapUser(user, owner);
    }

    private static UserResponseDto MapUser(User user, Owner? owner)
    {
        return new UserResponseDto
        {
            Id = user.Id,
            Username = user.Username,
            Role = user.Role,
            ScreenPermissions = DeserializeScreenPermissions(user.ScreenPermissionsJson),
            OwnerId = owner?.Id,
            OwnerFullName = owner?.FullName,
            CreatedAt = user.CreatedAt
        };
    }

    private static bool IsStaffRole(string role)
        => role.Equals("Admin", StringComparison.OrdinalIgnoreCase)
            || role.Equals("Employee", StringComparison.OrdinalIgnoreCase);

    private static bool IsKnownRole(string role)
        => role.Equals("Admin", StringComparison.OrdinalIgnoreCase)
            || role.Equals("Employee", StringComparison.OrdinalIgnoreCase)
            || role.Equals("OwnerClient", StringComparison.OrdinalIgnoreCase);

    private static List<string> ResolveScreenPermissions(string role, IEnumerable<string>? permissions, bool requireExisting)
    {
        if (!role.Equals("Employee", StringComparison.OrdinalIgnoreCase))
        {
            if (permissions != null && permissions.Any(p => !string.IsNullOrWhiteSpace(p)))
                throw new InvalidOperationException("Screen permissions can only be set for Employee users");

            return [];
        }

        var normalized = NormalizeScreenPermissions(permissions);
        if (normalized.Count == 0)
        {
            if (requireExisting || permissions != null)
                throw new InvalidOperationException("Employee users must have at least one visible screen");

            return [];
        }

        var invalid = normalized.FirstOrDefault(p => !EmployeeVisibleScreens.Contains(p));
        if (!string.IsNullOrWhiteSpace(invalid))
            throw new InvalidOperationException($"Unknown screen permission '{invalid}'");

        return normalized;
    }

    private static List<string> NormalizeScreenPermissions(IEnumerable<string>? permissions)
    {
        if (permissions == null) return [];

        return permissions
            .Select(p => p?.Trim())
            .Where(p => !string.IsNullOrWhiteSpace(p))
            .Distinct(StringComparer.OrdinalIgnoreCase)
            .ToList()!;
    }

    private static string SerializeScreenPermissions(IEnumerable<string> permissions)
        => JsonSerializer.Serialize(permissions.ToList());

    private static List<string> DeserializeScreenPermissions(string? json)
    {
        if (string.IsNullOrWhiteSpace(json)) return [];

        try
        {
            return JsonSerializer.Deserialize<List<string>>(json) ?? [];
        }
        catch
        {
            return [];
        }
    }
}
