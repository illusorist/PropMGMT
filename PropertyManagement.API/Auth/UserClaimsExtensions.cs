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


    // Returns user id from the standard NameIdentifier claim (if present)
    public static int? GetUserId(this ClaimsPrincipal user)
    {
        var value = user.FindFirstValue(ClaimTypes.NameIdentifier);
        return int.TryParse(value, out var id) ? id : null;
    }

    // Explicit admin helper for clearer checks where only Admin should have access
    public static bool IsAdmin(this ClaimsPrincipal user)
        => user.IsInRole("Admin");
}
