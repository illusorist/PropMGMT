using System.ComponentModel.DataAnnotations;

namespace PropertyManagement.Application.DTOs.User;

public class UserCreateDto
{
    [Required] public string Username { get; set; } = string.Empty;
    [Required] public string Password { get; set; } = string.Empty;
    [Required] public string Role { get; set; } = string.Empty;
}
