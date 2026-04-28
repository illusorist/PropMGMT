using System;
using System.Linq;

namespace PropertyManagement.Application.Services;

internal static class PasswordPolicy
{
    public static void EnsureStrong(string password)
    {
        if (string.IsNullOrWhiteSpace(password))
            throw new InvalidOperationException("Password is required");

        if (password.Length < 10)
            throw new InvalidOperationException("Password must be at least 10 characters long");

        if (!password.Any(char.IsUpper))
            throw new InvalidOperationException("Password must include at least one uppercase letter");

        if (!password.Any(char.IsLower))
            throw new InvalidOperationException("Password must include at least one lowercase letter");

        if (!password.Any(char.IsDigit))
            throw new InvalidOperationException("Password must include at least one number");

        if (!password.Any(ch => !char.IsLetterOrDigit(ch)))
            throw new InvalidOperationException("Password must include at least one special character");
    }
}
