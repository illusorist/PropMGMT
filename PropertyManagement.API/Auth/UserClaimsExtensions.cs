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

    public static bool IsAdminOrAgencyOwner(this ClaimsPrincipal user)
        => user.IsInRole("Admin") || user.IsInRole("AgencyOwner");
}
