using System.Collections.Generic;

namespace PropertyManagement.Application.DTOs.Auth;

public class AuthResponseDto
{
    public string Token { get; set; } = string.Empty;
    public string Username { get; set; } = string.Empty;
    public string Role { get; set; } = string.Empty;
    public List<string> ScreenPermissions { get; set; } = [];
}
