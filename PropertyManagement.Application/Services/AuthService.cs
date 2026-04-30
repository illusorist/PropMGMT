using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using PropertyManagement.Application.DTOs.Auth;
using PropertyManagement.Application.Interfaces;
using PropertyManagement.Domain.Entities;

namespace PropertyManagement.Application.Services;

public class AuthService
{
    private readonly IUserRepository _userRepo;
    private readonly IOwnerRepository _ownerRepo;
    private readonly IConfiguration _config;

    public AuthService(IUserRepository userRepo, IOwnerRepository ownerRepo, IConfiguration config)
    {
        _userRepo = userRepo;
        _ownerRepo = ownerRepo;
        _config = config;
    }

    public async Task<AuthResponseDto?> LoginAsync(LoginDto dto)
    {
        var user = await _userRepo.GetByUsernameAsync(dto.Username);
        if (user == null || !BCrypt.Net.BCrypt.Verify(dto.Password, user.PasswordHash))
            return null;

        var owner = await _ownerRepo.GetByUserIdAsync(user.Id);
        var token = GenerateToken(user, owner?.Id);
        return new AuthResponseDto { Token = token, Username = user.Username, Role = user.Role, ScreenPermissions = ReadScreenPermissions(user) };
    }

    private string GenerateToken(User user, int? ownerId)
    {
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]!));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new Claim(ClaimTypes.Name, user.Username),
            new Claim(ClaimTypes.Role, user.Role),
            new Claim("screen_permissions", JsonSerializer.Serialize(ReadScreenPermissions(user)))
        };
        if (ownerId.HasValue)
        {
            claims.Add(new Claim("owner_id", ownerId.Value.ToString()));
        }
        var token = new JwtSecurityToken(
            issuer: _config["Jwt:Issuer"],
            audience: _config["Jwt:Audience"],
            claims: claims,
            expires: DateTime.UtcNow.AddDays(7),
            signingCredentials: creds
        );
        return new JwtSecurityTokenHandler().WriteToken(token);
    }

    private static List<string> ReadScreenPermissions(User user)
    {
        if (string.IsNullOrWhiteSpace(user.ScreenPermissionsJson)) return [];

        try
        {
            return JsonSerializer.Deserialize<List<string>>(user.ScreenPermissionsJson) ?? [];
        }
        catch
        {
            return [];
        }
    }
}
