using System.Text.Json;
using System.Collections.Generic;
using System.Security.Claims;

namespace PropertyManagement.API.Auth;

public static class UserClaimsExtensions
{
    public static int? GetOwnerId(this ClaimsPrincipal user)
    {
        var value = user.FindFirstValue("owner_id");
        return int.TryParse(value, out var id) ? id : null;
    }

    public static bool IsOwnerClient(this ClaimsPrincipal user)
        => user.IsInRole("OwnerClient");

    public static bool IsStaff(this ClaimsPrincipal user)
        => user.IsInRole("Admin") || user.IsInRole("Employee");

    public static bool HasScreenPermission(this ClaimsPrincipal user, string screenPath)
    {
        if (user.IsAdmin()) return true;

        var value = user.FindFirstValue("screen_permissions");
        if (string.IsNullOrWhiteSpace(value)) return false;

        try
        {
            var permissions = JsonSerializer.Deserialize<List<string>>(value) ?? [];
            return permissions.Exists(path =>
                !string.IsNullOrWhiteSpace(path)
                && string.Equals(path.Trim(), screenPath, StringComparison.OrdinalIgnoreCase));
        }
        catch
        {
            return false;
        }
    }


    // Returns user id from the standard NameIdentifier claim (if present)
    public static int? GetUserId(this ClaimsPrincipal user)
    {
        var value = user.FindFirstValue(ClaimTypes.NameIdentifier);
        return int.TryParse(value, out var id) ? id : null;
    }

    // Explicit admin helper for screens or actions that must stay admin-only.
    public static bool IsAdmin(this ClaimsPrincipal user)
        => user.IsInRole("Admin");
}
