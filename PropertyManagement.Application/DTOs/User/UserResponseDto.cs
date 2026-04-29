namespace PropertyManagement.Application.DTOs.User;

public class UserResponseDto
{
    public int Id { get; set; }
    public string Username { get; set; } = string.Empty;
    public string Role { get; set; } = string.Empty;
    public int? OwnerId { get; set; }
    public string? OwnerFullName { get; set; }
    public DateTime CreatedAt { get; set; }
}
