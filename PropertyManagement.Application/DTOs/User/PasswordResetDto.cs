using System.ComponentModel.DataAnnotations;

namespace PropertyManagement.Application.DTOs.User;

public class PasswordResetDto
{
    [Required]
    public string Password { get; set; } = string.Empty;
}
