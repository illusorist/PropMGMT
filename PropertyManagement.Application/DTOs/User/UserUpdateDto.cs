using System.Collections.Generic;

namespace PropertyManagement.Application.DTOs.User;

public class UserUpdateDto
{
    public string? Username { get; set; }
    public string? Password { get; set; }
    public string? Role { get; set; }
    public List<string>? ScreenPermissions { get; set; }
}
