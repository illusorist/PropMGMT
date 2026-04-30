using System.Collections.Generic;

namespace PropertyManagement.Application.DTOs.User;

public class UserResponseDto
{
    public int Id { get; set; }
    public string Username { get; set; } = string.Empty;
    public string Role { get; set; } = string.Empty;
    public List<string> ScreenPermissions { get; set; } = [];
    public int? OwnerId { get; set; }
    public string? OwnerFullName { get; set; }
    public DateTime CreatedAt { get; set; }
}
