using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using PropertyManagement.Domain.Entities;
using PropertyManagement.Infrastructure.Data;

namespace PropertyManagement.API.Startup;

public sealed class BootstrapAdminInitializer
{
    private readonly AppDbContext _db;
    private readonly IConfiguration _configuration;
    private readonly ILogger<BootstrapAdminInitializer> _logger;

    public BootstrapAdminInitializer(
        AppDbContext db,
        IConfiguration configuration,
        ILogger<BootstrapAdminInitializer> logger)
    {
        _db = db;
        _configuration = configuration;
        _logger = logger;
    }

    public async Task EnsureBootstrapAdminAsync()
    {
        var enabled = _configuration.GetValue<bool>("BootstrapAdmin:Enabled");
        if (!enabled)
        {
            _logger.LogInformation("Bootstrap admin creation is disabled.");
            return;
        }

        var username = _configuration["BootstrapAdmin:Username"]?.Trim();
        var password = _configuration["BootstrapAdmin:Password"];

        if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password))
        {
            _logger.LogWarning("Bootstrap admin is enabled but username/password are missing.");
            return;
        }

        var hasAdmin = await _db.Users.AnyAsync(u => u.Role == "Admin");
        if (hasAdmin)
        {
            _logger.LogInformation("Admin user already exists. Skipping bootstrap admin creation.");
            return;
        }

        var hasAnyUserWithUsername = await _db.Users.AnyAsync(u => u.Username == username);
        if (hasAnyUserWithUsername)
        {
            _logger.LogWarning("Bootstrap admin username '{Username}' already exists with a different role. Skipping creation.", username);
            return;
        }

        var user = new User
        {
            Username = username,
            PasswordHash = BCrypt.Net.BCrypt.HashPassword(password),
            Role = "Admin",
            CreatedAt = DateTime.UtcNow
        };

        await _db.Users.AddAsync(user);
        await _db.SaveChangesAsync();

        _logger.LogInformation("Bootstrap admin user created successfully.");
    }
}
